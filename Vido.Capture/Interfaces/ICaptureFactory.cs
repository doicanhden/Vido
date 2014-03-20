namespace Vido.Capture.Interfaces
{
  public interface ICaptureFactory
  {
    ICapture Create(CaptureConfigs configs);
  }
}
