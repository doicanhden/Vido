namespace Vido
{
  using System;
  using System.IO;

  public interface IImageHolder : IDisposable
  {
    bool Available { get; set; }
    IImageHolder Copy();
    bool Load(Stream stream);
    bool Save(Stream stream);
  }
}
