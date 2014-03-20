namespace Vido.Parking
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.IO;
  using System.Threading;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Controls;
  using Vido.Parking.Interfaces;

  public static class Utilites
  {
    public static string EntryString
    {
      get { return ("0"); }
    }
    public static string ExitString
    {
      get { return ("1"); }
    }

    public static string SuffixPlateFileName
    {
      get { return ("P.jpg"); }
    }
    public static string SuffixFaceFileName
    {
      get { return ("F.jpg"); }
    }

    public static string RootImageDirectoryName
    {
      get { return (@"F:\Khanh\SkyDrive\Development\Github\Vido's Projects\Vido\RootImages"); }
    }
    public static string DailyDirectoryNameFormat
    {
      get
      {
        if (Path.DirectorySeparatorChar == '\\')
        {
          return (@"yyyy\\MM\\dd");
        }

        return ("yyyy/MM/dd");
      }
    }
    public static string DailyDirectoryName(DateTime time)
    {
      return (RootImageDirectoryName +
        Path.DirectorySeparatorChar +
        time.ToString(DailyDirectoryNameFormat));
    }

    public static bool TakeAndSave(ICapture camera, string fileName)
    {
      for (int i = 0; i < 3; ++i)
      {
        var image = camera.Take() as Bitmap;
        if (image != null)
        {
          image.Save(fileName);
          return (true);
        }

        Thread.Sleep(250);
      }

      return (false);
    }

    public static bool CanEntry(IDatabase database, string cardId, string plateNumber)
    {
      return (Controls.Card.IsUse(database, cardId));
    }
    public static bool Entry(IDatabase database, string cardId, Lane lane)
    {
      var time = DateTime.Now;
      var dailyDirectoryName = DailyDirectoryName(time);

      if (!Directory.Exists(dailyDirectoryName))
      {
        Directory.CreateDirectory(dailyDirectoryName);
      }

      var prefixFileName = dailyDirectoryName +
        Path.DirectorySeparatorChar +
        time.ToString("HHmmss") + EntryString;

      var plateNumber = string.Empty;
      var plateFileName = string.Empty;
      var faceFileName = string.Empty;

      if (lane.BackCamera != null)
      {
        plateFileName = prefixFileName + SuffixPlateFileName;
        if (!TakeAndSave(lane.BackCamera, plateFileName))
        {
          // Error
        }

        // Todo: Get Plate number.
      }
      else
      {
        // Error
      }

      if (lane.FrontCamera != null)
      {
        faceFileName = prefixFileName + SuffixFaceFileName;
        if (!TakeAndSave(lane.FrontCamera, faceFileName))
        {
          faceFileName = string.Empty;
        }
      }

      if (CanEntry(database, cardId, plateNumber))
      {
        var paramater = new Dictionary<string, string>();
        paramater["CardId"] = cardId;
        paramater["EntryTime"] = time.ToString();
        paramater["EntryPlateNumber"] = plateNumber;
        paramater["EntryPlateImage"] = plateFileName.Remove(0, RootImageDirectoryName.Length);
        paramater["EntryFaceImage"] = faceFileName.Remove(0, RootImageDirectoryName.Length);

        return (database.Insert("EntryExit", paramater));
      }

      return (false);
    }

    public static bool CanExit(IDatabase database, string cardId, string plateNumber)
    {
      return (Controls.Card.IsUse(database, cardId));
    }
    public static bool Exit(IDatabase database, string cardId, Lane lane)
    {
      var time = DateTime.Now;
      var dailyDirectoryName = DailyDirectoryName(time);

      if (!Directory.Exists(dailyDirectoryName))
      {
        Directory.CreateDirectory(dailyDirectoryName);
      }

      var prefixFileName = dailyDirectoryName +
        Path.DirectorySeparatorChar +
        time.ToString("HHmmss") + ExitString;

      var plateNumber = string.Empty;
      var plateFileName = string.Empty;
      var faceFileName = string.Empty;

      if (lane.BackCamera != null)
      {
        plateFileName = prefixFileName + SuffixPlateFileName;
        if (!TakeAndSave(lane.BackCamera, plateFileName))
        {
          // Error
        }

        // Todo: Get Plate number.
      }
      else
      {
        // Error
      }

      if (lane.FrontCamera != null)
      {
        faceFileName = prefixFileName + SuffixFaceFileName;
        if (!TakeAndSave(lane.FrontCamera, faceFileName))
        {
          faceFileName = string.Empty;
        }
      }

      if (CanExit(database, cardId, plateNumber))
      {
        var paramater = new Dictionary<string, string>();
        //      paramater["CardId"] = cardId;
        paramater["ExitTime"] = time.ToString();
        paramater["ExitPlateNumber"] = plateNumber;
        paramater["ExitPlateImage"] = plateFileName;
        paramater["ExitFaceImage"] = faceFileName;

        return (database.Insert("EntryExit", paramater));
      }

      return (false);
    }
  }
}
