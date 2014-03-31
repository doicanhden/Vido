namespace Vido.Parking.Desktop
{
  using System;
  using System.IO;

  public class DailyDirectory : IDailyDirectory
  {

    public string RootDirectoryName { get; set; }

    public bool Create(DateTime time)
    {
      throw new System.NotImplementedException();
    }

    public Stream NewFile(string name)
    {
      
      var fileName = RootDirectoryName +
        Path.DirectorySeparatorChar + name;

      return (new FileStream(fileName, FileMode.CreateNew));
    }
  }
}
