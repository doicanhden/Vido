namespace Vido.Desktop
{
  using System.IO;

  public class FileStorage : IFileStorage
  {
    public Stream Open(string fileName)
    {
      return (File.Open(fileName, FileMode.Open));
    }

    public bool Exists(string fileName)
    {
      return (File.Exists(fileName));
    }

    public void Delete(string fileName)
    {
      File.Delete(fileName);
    }
  }
}
