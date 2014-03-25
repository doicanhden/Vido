using System.Collections.Generic;
namespace Vido.Capture.Interfaces
{
  public interface ICaptureList : ICaptureFactory
  {
    ICollection<ICapture> Captures { get; }
  }
}
