namespace Vido.Capture.Interfaces
{
  using System.Drawing;
  using System.Threading;

  public interface IQueryFrame
  {
    Image Query(ICapture capture, WaitHandle stopEvent = null, WaitHandle reloadEvent = null);
  }
}
