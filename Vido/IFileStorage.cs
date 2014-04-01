namespace Vido
{
  using System.IO;

  public interface IFileStorage
  {
    Stream Open(string fileName);
    bool Exists(string fileName);
    void Delete(string fileName);
  }
}
