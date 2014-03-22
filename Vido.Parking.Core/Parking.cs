namespace Vido.Parking
{
  using System;
  using System.Drawing;
  using System.IO;
  using System.Text;
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
      
      var sb = new StringBuilder();
      sb.Append(time.ToString(DailyDirectoryNameFormat));
      sb.Append(Path.DirectorySeparatorChar);
      sb.Append(time.ToString("HHmmss"));
      sb.Append(Convert.ToBase64String(data));
      sb.Append('I');
      sb.Append(plateNumber);

      var prefixName = PrefixName(time) + Convert.ToBase64String(data) +  '0';

    }
    
    private static void SaveImage(Image image, string name)
    {
      var fileName = RootImageDirectoryName + name;
      image.Save(fileName);
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
