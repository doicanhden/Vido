namespace Vido.Parking
{
  using System;
  using System.IO;

  public interface IDailyDirectory
  {
    public string FormatString { get; set; }
    public string RootDirectoryName { get; set; }
    bool Create(DateTime time);
    Stream NewFile(string name);
  }
}
