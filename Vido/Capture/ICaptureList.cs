namespace Vido.Capture
{
  using System.Collections.Generic;

  public interface ICaptureList : ICaptureFactory
  {
    ICollection<ICapture> Captures { get; }
  }
}