namespace Vido.Capture
{
  using System.Collections.Generic;

  public interface ICaptureList : IFactory
  {
    ICollection<ICapture> Captures { get; }
  }
}