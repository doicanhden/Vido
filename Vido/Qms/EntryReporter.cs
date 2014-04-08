// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using Vido.Media;

  public class EntryReporter : IEntryReporter
  {
    private readonly List<TaskState> tasks;
    private readonly ReporterServices services;
    private readonly IEntryRecorder recorder;
    private readonly IUniqueIdStorage idStorage;
    private readonly IInputDeviceList inputList;

    public EntryReporter(
      ReporterServices services,
      IInputDeviceList inputList,
      IUniqueIdStorage idStorage,
      IEntryRecorder recorder)
    {
      this.tasks = new List<TaskState>();
      this.services = services;
      this.inputList = inputList;
      this.idStorage = idStorage;
      this.recorder = recorder;
    }

    public IDisposable Register(IGate gate)
    {
      if (!tasks.Exists((g) => g.Gate == gate))
      {
        gate.Input.DataIn += Input_DataIn;
        gate.Input.Deregister = inputList.Register(gate.Input);

        tasks.Add(new TaskState(gate, WorkerThread));
      }

      return (new Deregister<IGate>(gate, (g) =>
      {
        var task = tasks.Find((t) => t.Gate == g);
        if (task != null)
        {
          tasks.Remove(task);

          gate.Input.DataIn -= Input_DataIn;
          gate.Input.Deregister.Dispose();
          gate.Input.Deregister = null;

          task.Close();
        }
      }));
    }

    #region Private Methods
    private void Input_DataIn(object sender, EventArgs e)
    {
      var args = e as DataInEventArgs;
      var task = tasks.Find((t) => t.Gate.Input == sender);

      if (task != null)
      {
        var gate = task.Gate;
        var images = gate.CaptureImages();

        if (images != null)
        {
          var entryArgs = new EntryArgs()
          {
            Gate = gate,
            UniqueId = services.GetUniqueId(args.Data, args.Printable),
            UserData = services.GetUserData(images),
            Images = images
          };
          gate.RasieNewEntries(entryArgs);

          task.Entries.Enqueue(entryArgs);
          task.NewEntries.Set();
        }
        else
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          gate.RaiseNewMessage("Không chụp được ảnh");
        }
      }
    }

    private void WorkerThread(TaskState state)
    {
      var gate = state.Gate;
      var entries = state.Entries;
      var stopTask = state.StopTask;
      var entryBlock = state.EntryBlock;
      var entryAllow = state.EntryAllow;
      var newEntries = state.NewEntries;

      EntryArgs cur = null;
      while (stopTask.WaitOne(10))
      {
        if (entries.TryDequeue(out cur))
        {
          var uniqueId = cur.UniqueId.UniqueId;
          if (idStorage.CanUse(uniqueId))
          {
            if (gate.Direction == Direction.Import)
            {
              #region Import Processes
              if (!recorder.IsFull)
              {
                bool allow = services.EntryRequest(entryBlock, newEntries, entryAllow);

                /// Sao chép chuỗi, .Net ngu ngốc. T_+.
                var userData = cur.UserData.UserData.Substring(0);
                var entry = new Entry()
                {
                  EntryGate = gate.Name,
                  EntryTime = DateTime.Now,
                  UniqueId = uniqueId,
                  UserData = userData
                };

                if (services.SaveImage(cur.Images, entry, gate.Direction))
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
              }
              else
              {
                /// TODO: Địa phương hóa chuỗi thông báo.
                gate.RaiseNewMessage("Đầy");
              }
              #endregion
            }
            else
            {
              #region Export Processes
              /// Sao chép chuỗi, .Net ngu ngốc. T_+.
              var userData = cur.UserData.UserData.Substring(0);
              var entry = new Entry()
              {
                EntryGate = gate.Name,
                EntryTime = DateTime.Now,
                UniqueId = uniqueId,
                UserData = userData
              };

              if (services.SaveImage(cur.Images, entry, gate.Direction))
              {
                string first, second;
                if (recorder.CanExport(uniqueId, userData, out first, out second))
                {
                  /// Tao đ' cần biết mày làm thế nào,
                  /// tao đưa đường dẫn đấy. Tự mà load ảnh. -_-
                  gate.RaiseSavedImage(services.ImageRoot, first, second);

                  if (services.EntryRequest(entryBlock, newEntries, entryAllow))
                  {
                    if (recorder.Export(entry))
                    {
                      gate.RaiseEntryAllow(userData);
                    }
                    else
                    {
                      /// TODO: Địa phương hóa chuỗi thông báo.
                      gate.RaiseNewMessage("Ghi dữ liệu thất bại");
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
          }
          else
          {
            /// TODO: Địa phương hóa chuỗi thông báo.
            gate.RaiseNewMessage("Id không hợp lệ");
          }

          if (entries.Count <= 0)
          {
            newEntries.Reset();
          }
        }
      }
    }
    #endregion
  }
}