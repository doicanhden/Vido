namespace Vido
{
  using System;
  using System.IO;

  public interface IImageHolder : IDisposable
  {
    bool Available { get; }

    bool Load(IFileStorage storage, string fileName);
    bool Save(IFileStorage storage, string fileName);
    bool Load(Stream stream);
    bool Save(Stream stream);
    IImageHolder Copy();
  }
}
