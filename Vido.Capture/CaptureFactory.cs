namespace Vido.Capture
{
  using Vido.Capture.Enums;
  using Vido.Capture.Interfaces;
  public class CaptureFactory : ICaptureFactory
  {
    public ICapture Create(CaptureConfigs configs)
    {
      switch (configs.Coding)
      {
        case Coding.Jpeg:
          return (new JpegStream());
        case Coding.MJpeg:
          return (new MJpegStream());
        default:
          return (null);
      }
    }
  }
}
