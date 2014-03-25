namespace Vido.Parking
{
  using System;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.IO;
  using System.Text;
  using Vido.Parking.Interfaces;
  using Vido.Parking.Utilities;
  using System.Linq;

  public class Parking : IParking
  {
    private VidoParkingEntities entities = new VidoParkingEntities();
    
    public ISettingsProvider Settings { get; set; }

    public bool AddCard(byte[] data, long CardTypeId, bool isUse = true)
    {
      try
      {
        entities.Card.Add(new Card()
        {
          CardId = EncodeData(data),
          CardTypeId = CardTypeId,
          IsUse = (isUse ? 1 : 0)
        });

        return (true);
      }
      catch
      {
        throw;
      }
    }
    public bool IsUse(byte[] data)
    {
      var isUses = from Cards in entities.Card
                   where Cards.CardId == EncodeData(data)
                   select Cards.IsUse;

      return (!(isUses == null || isUses.All((x) => x.Value == 0)));
    }
    public bool CanExit(byte[] data, string plateNumber)
    {
      return (true);
    }

    public void Exit(byte[] data, string plateNumber, Image frontImage, Image backImage)
    {
      DateTime time = DateTime.Now;

      var dailyDirectoryName = DailyDirectoryName(time);
      CreateDirectoryIfNotExists(dailyDirectoryName);

      var timeString = time.ToString("HHmmss");
      var dataBase64 = EncodeData(data);

      dailyDirectoryName += Path.DirectorySeparatorChar;

      var backImageFileName  = dailyDirectoryName + string.Format(BackImageNameFormat , timeString, OutFormat, plateNumber, dataBase64);
      var frontImageFileName = dailyDirectoryName + string.Format(FrontImageNameFormat, timeString, OutFormat, plateNumber, dataBase64);

      if (backImage != null)
      {
        backImage.Save(backImageFileName);
      }
      if (frontImage != null)
      {
        frontImage.Save(frontImageFileName);
      }

      /// TODO: Update into DB.
      /// ExitBackImage = frontImageFileName.Substring(0, RootImageDirectoryName.Length);
      /// ExitFrontImage = frontImageFileName.Substring(0, RootImageDirectoryName.Length);
    }

    public bool CanEntry(byte[] data, string plateNumber)
    {
      var isUses = (
        from cards in entities.Card
        where cards.CardId == EncodeData(data) && (
          from entryExit in cards.EntryExit
          where entryExit.CardId != cards.CardId
          select entryExit) == null
        select cards.IsUse);

      return (true);
    }
    
    public void Entry(byte[] data, string plateNumber, Image frontImage, Image backImage)
    {
      DateTime time = DateTime.Now;

      var dailyDirectoryName = DailyDirectoryName(time);
      CreateDirectoryIfNotExists(dailyDirectoryName);

      var timeString = time.ToString("HHmmss");
      var dataBase64 = EncodeData(data);

      dailyDirectoryName += Path.DirectorySeparatorChar;

      var backImageFileName  = dailyDirectoryName + string.Format(BackImageNameFormat , timeString, InFormat, plateNumber, dataBase64);
      var frontImageFileName = dailyDirectoryName + string.Format(FrontImageNameFormat, timeString, InFormat, plateNumber, dataBase64);

      if (backImage != null)
      {
        backImage.Save(backImageFileName);
      }
      if (frontImage != null)
      {
        frontImage.Save(frontImageFileName);
      }

      /// TODO: Update into DB.
      /// EntryBackImage = frontImageFileName.Substring(0, RootImageDirectoryName.Length);
      /// EntryFrontImage = frontImageFileName.Substring(0, RootImageDirectoryName.Length);
    }
    private void CreateDirectoryIfNotExists(string directoryName)
    {
      if (!Directory.Exists(directoryName))
      {
        Directory.CreateDirectory(directoryName);
      }
    }
    private static string EncodeData(byte[] data)
    {
      return (Convert.ToBase64String(data));
    }

    private string DailyDirectoryName(DateTime time)
    {
      var dailyDirectoryName = string.Empty;

      if (Path.DirectorySeparatorChar == '\\')
        dailyDirectoryName = string.Format(DailyDirectoryFormat, @"\\");
      else
        dailyDirectoryName = string.Format(DailyDirectoryFormat, Path.DirectorySeparatorChar);

      return (RootImageDirectoryName + time.ToString(dailyDirectoryName));
    }

    #region Setting Accessors
    private string InFormat
    {
      get { return (Settings.Query<string>(SettingKeys.InFormat)); }
    }
    private string OutFormat
    {
      get { return (Settings.Query<string>(SettingKeys.OutFormat)); }
    }
    private string RootImageDirectoryName
    {
      get { return (Settings.Query<string>(SettingKeys.RootImageDirectoryName)); }
    }
    private string DailyDirectoryFormat
    {
      get { return (Settings.Query<string>(SettingKeys.DailyDirectoryFormat)); }
    }
    public string FrontImageNameFormat
    {
      get { return (Settings.Query<string>(SettingKeys.FrontImageNameFormat)); }
    }
    public string BackImageNameFormat
    {
      get { return (Settings.Query<string>(SettingKeys.BackImageNameFormat)); }
    }
    #endregion
  }
}
