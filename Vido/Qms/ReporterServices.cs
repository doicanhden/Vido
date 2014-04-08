// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;
  using System.Text;
  using System.Threading;
  using Vido.Media;

  public class ReporterServices
  {
    public IDailyDirectory ImageRoot { get; set; }
    public int EntryRequestTimeout { get; set; }
    public string ImportString { get; set; }
    public string ExportString { get; set; }

    /// Time, Gate, Im/Ex, Unique Id, User data, Index.
    public string ImageNameFormat { get; set; }

    public ReporterServices()
    {
      this.ImageRoot = null;
      this.EntryRequestTimeout = 15000;
      this.ImportString = "IM";
      this.ExportString = "EX";
      this.ImageNameFormat = "IMG_{0:HHmmss}{1}{2}_{3}{4}{5}.jpg";
    }

    public virtual IUniqueId GetUniqueId(byte[] uniqueId, bool printable)
    {
      return (new UniqueId(printable ?
        Encoding.Unicode.GetString(uniqueId, 0, uniqueId.Length) :
        Convert.ToBase64String(uniqueId)));
    }

    public virtual IUserData GetUserData(ImagePair image)
    {
      return (new UserData(string.Empty));
    }

    public virtual bool EntryRequest(
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

    public virtual bool SaveImage(ImagePair image, Entry entry, Direction direction)
    {
      var imEx = direction == Direction.Import ? ImportString : ExportString;

      if (image.First != null && image.First.Available)
      {
        var path = ImageRoot.GetPath(entry.EntryTime, string.Format(ImageNameFormat,
          entry.EntryTime, entry.EntryGate, imEx, entry.UniqueId, entry.UserData, 0));

        if (image.First.Save(ImageRoot, path))
        {
          entry.BackImage = path;
        }
      }

      if (image.Second != null && image.Second.Available)
      {
        var path = ImageRoot.GetPath(entry.EntryTime, string.Format(ImageNameFormat,
          entry.EntryTime, entry.EntryGate, imEx, entry.UniqueId, entry.UserData, 1));

        if (image.Second.Save(ImageRoot, path))
        {
          entry.FrontImage = path;
        }
      }

      return (true);
    }
  }
}