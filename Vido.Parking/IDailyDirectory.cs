namespace Vido.Parking
{
  using System;
  using System.IO;

  public interface IDailyDirectory : IFileStorage
  {
    string RootDirectoryName { get; set; }
    
    Stream FileNew(DateTime time, string name, out string fileName);
  }
}
