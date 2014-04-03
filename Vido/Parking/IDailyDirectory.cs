namespace Vido.Parking
{
  using System;
  using System.IO;

  public interface IDailyDirectory : IFileStorage
  {
    string RootDirectoryName { get; set; }
    string GetFullPath(DateTime time, string name);
  }
}
