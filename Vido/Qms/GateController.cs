// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;

  public class GateController : IDisposable
  {
    private readonly List<TaskState> tasks;
    private readonly IDailyDirectory rootImages;
    private readonly IEntryRecorder recorder;

    public GateController(
      IGateServices gateServices,
      IEntryRecorder recorder,
      IDailyDirectory rootImages)
    {
      this.tasks = new List<TaskState>();
      this.rootImages = rootImages;
      this.recorder = recorder;

      foreach (var gate in gateServices.GetAllGates(this))
      {
        tasks.Add(new TaskState(gate, WorkerThread));
      }
    }

    public int EntryRequestTimeout { get; set; }
    public string ImportString { get; set; }
    public string ExportString { get; set; }
    public string BackImageNameFormat { get; set; }
    public string FrontImageNameFormat { get; set; }

    public void NewEntries(EntryArgs entryArgs)
    {
      foreach (var task in tasks)
      {
        if (task.Gate == entryArgs.Gate)
        {
          task.Entries.Enqueue(entryArgs);
          task.NewEntries.Set();
          break;
        }
      }
    }

    #region Private Methods
    private bool EntryRequest(
      EventWaitHandle entryBlock,
      EventWaitHandle newEntries,
      EventWaitHandle entryAllow)
    {
      bool allow = true;

      for (int t = EntryRequestTimeout; t > 0; t -= 1000)
      {
        if (entryBlock.WaitOne(500))
        {
          allow = false;
          break;
        }

        if (newEntries.WaitOne(0))
        {
          break;
        }

        if (entryAllow.WaitOne(500))
        {
          break;
        }
      }

      return (allow);
    }

    private bool SaveImage(ImagePair image, Entry entry, Direction direction)
    {
      var imEx = direction == Direction.Import ? ImportString : ExportString;
      var strTime = entry.EntryTime.ToString("HHmmss");

      if (image.First != null && image.First.Available)
      {
        var path = rootImages.GetPath(entry.EntryTime, string.Format(BackImageNameFormat,
          strTime, entry.UniqueId, imEx, entry.EntryGate, entry.UserData));

        if (image.First.Save(rootImages, path))
        {
          entry.BackImage = path;
        }
      }

      if (image.Second != null && image.Second.Available)
      {
        var path = rootImages.GetPath(entry.EntryTime, string.Format(FrontImageNameFormat,
          strTime, entry.UniqueId, imEx, entry.EntryGate, entry.UserData));

        if (image.Second.Save(rootImages, path))
        {
          entry.FrontImage = path;
        }
      }

      return (true);
    }

    private void WorkerThread(TaskState obj)
    {
      var entryBlock = obj.EntryBlock;
      var entryAllow = obj.EntryAllow;
      var newEntries = obj.NewEntries;
      var stopProcess = obj.StopProcess;
      var gate = obj.Gate;
      var entries = obj.Entries;

      EntryArgs entryArgs = null;
      while (stopProcess.WaitOne(10))
      {
        if (entries.TryDequeue(out entryArgs))
        {
          if (gate.Direction == Direction.Import)
          {
            #region Import Processes
            bool allow = EntryRequest(entryBlock, newEntries, entryAllow);

            /// Sao chép chuỗi, .Net ngu ngốc. T_+.
            var userData = entryArgs.UserData.UserData.ToUpper();
            var uniqueId = entryArgs.UniqueId.ToPrintable();
            var entry = new Entry()
            {
              EntryGate = gate.Name,
              EntryTime = DateTime.Now,
              UniqueId = uniqueId,
              UserData = userData
            };

            if (SaveImage(entryArgs.Images, entry, gate.Direction))
            {
              if (allow && recorder.CanImport(uniqueId, userData))
              {
                if (recorder.Import(entry))
                {
                  gate.RaiseEntryAllow(userData);

                  /// TODO: Add Printer
                }
                else
                {
                  /// TODO: Địa phương hóa chuỗi thông báo.
                  gate.RaiseNewMessage("Ghi dữ liệu thất bại.");
                }
              }
              else
              {
                recorder.Blocked(entry);
                gate.RaiseEntryBlock(userData);
              }
            }
            else
            {
              /// TODO: Địa phương hóa chuỗi thông báo.
              gate.RaiseNewMessage("Không lưu được ảnh");
            }
            #endregion
          }
          else
          {
            #region Export Processes
            /// Sao chép chuỗi, .Net ngu ngốc. T_+.
            var userData = entryArgs.UserData.UserData.ToUpper();
            var uniqueId = entryArgs.UniqueId.ToPrintable();
            var entry = new Entry()
            {
              EntryGate = gate.Name,
              EntryTime = DateTime.Now,
              UniqueId = uniqueId,
              UserData = userData
            };

            if (SaveImage(entryArgs.Images, entry, gate.Direction))
            {
              string first, second;
              if (recorder.CanExport(uniqueId, userData, out first, out second))
              {
                /// Tao đ' cần biết mày làm thế nào,
                /// tao đưa đường dẫn đấy. Tự mà load ảnh. -_-
                gate.RaiseSavedImage(rootImages, first, second);

                if (EntryRequest(entryBlock, newEntries, entryAllow))
                {
                  if (recorder.Export(entry))
                  {
                    gate.RaiseEntryAllow(userData);
                  }
                  else
                  {
                    /// TODO: Địa phương hóa chuỗi thông báo.
                    gate.RaiseNewMessage("Ghi dữ liệu thất bại.");
                  }
                }
                else
                {
                  recorder.Blocked(entry);
                  gate.RaiseEntryBlock(userData);
                }
              }
              else
              {
                recorder.Blocked(entry);
                gate.RaiseEntryBlock(userData);
              }
            }
            else
            {
              /// TODO: Địa phương hóa chuỗi thông báo.
              gate.RaiseNewMessage("Không lưu được ảnh");
            }
            #endregion
          }

          if (entries.Count <= 0)
          {
            newEntries.Reset();
          }
        }
      }
    }
    #endregion

    #region Implementation of IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        foreach (var task in tasks)
        {
          task.StopProcess.Set();
        }

        tasks.Clear();
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion
  }

  class TaskState
  {
    private readonly Task task;
    public IGate Gate { get; private set; }
    public ConcurrentQueue<EntryArgs> Entries { get; private set; }
    public EventWaitHandle EntryAllow { get; private set; }
    public EventWaitHandle EntryBlock { get; private set; }
    public EventWaitHandle NewEntries { get; private set; }
    public EventWaitHandle StopProcess { get; private set; }

    public TaskState(IGate gate, Action<TaskState> workerThread)
    {
      this.Gate = gate;
      this.EntryAllow = gate.Allow;
      this.EntryBlock = gate.Block;
      this.NewEntries = new ManualResetEvent(false);
      this.StopProcess = new ManualResetEvent(false);
      this.Entries = new ConcurrentQueue<EntryArgs>();

      this.task = new Task((x) => workerThread(x as TaskState), this);

      this.task.Start();
    }
  }
}