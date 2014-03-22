namespace Vido.Parking
{
  using System;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.IO;
  using System.Text;
  using Vido.Parking.DataSet;
  using Vido.Parking.Interfaces;
  using Vido.Parking.Utilities;

  public class Parking : IParking
  {
    public bool CanExit(byte[] data, string plateNumber)
    {
      return (true);
    }

    public void Exit(byte[] data, string plateNumber, Image frontImage, Image backImage)
    {
    }

    public bool CanEntry(byte[] data, string plateNumber)
    {
      return (true);
    }
    
    public void Entry(byte[] data, string plateNumber, Image frontImage, Image backImage)
    {
      DateTime time = DateTime.Now;

      // Tạo thư mục chứa ảnh.
      CreateDailyDirectory(time);

      var prefixName = PrefixName(time, data, plateNumber, 'I');
      var frontImageFileName = prefixName + "F.jpg";
      var backImageFileName = prefixName + "B.jpg";

      SaveImage(frontImage, frontImageFileName);
      SaveImage(backImage , backImageFileName);

    }
    private static string PrefixName(DateTime time, byte[] uidData, string plateNumber, char io)
    {
      var sb = new StringBuilder();
      sb.Append(time.ToString(DailyDirectoryNameFormat));
      sb.Append(Path.DirectorySeparatorChar);
      sb.Append(time.ToString("HHmmss"));
      sb.Append(Convert.ToBase64String(uidData));
      sb.Append(io);
      sb.Append(plateNumber);

      return (sb.ToString());
    }
    
    private static bool SaveImage(Image image, string subFileName)
    {
      var fileName = RootImageDirectoryName + subFileName;
      image.Save(fileName, ImageFormat.Jpeg);

      return (File.Exists(fileName));
    }

    private static void CreateDailyDirectory(DateTime time)
    {
      var dailyDirectoryName = DailyDirectoryName(time);
      if (!Directory.Exists(dailyDirectoryName))
      {
        Directory.CreateDirectory(dailyDirectoryName);
      }
    }
    private static string DailyDirectoryName(DateTime time)
    {
      return (RootImageDirectoryName + time.ToString(DailyDirectoryNameFormat));
    }
    private static string PrefixName(DateTime time)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append(time.ToString(DailyDirectoryNameFormat));
      sb.Append(Path.DirectorySeparatorChar);
      sb.Append(time.ToString("HHmmss"));

      return (sb.ToString());
    }

    private static string DailyDirectoryNameFormat
    {
      get
      {
        if (Path.DirectorySeparatorChar == '\\')
        {
          return (@"\\yyyy\\MM\\dd");
        }

        return (string.Format("{0}yyyy{0}MM{0}dd", Path.DirectorySeparatorChar));
      }
    }
    private static string RootImageDirectoryName
    {
      get { return Properties.Settings.Default.RootImageDirectoryName; }
      set { Properties.Settings.Default.RootImageDirectoryName = value; }
    }
  }
}
