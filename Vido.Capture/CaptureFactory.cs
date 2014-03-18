namespace Vido.Capture
{
  using Vido.Capture.Enums;
  using Vido.Capture.Interfaces;
  public class CaptureFactory : ICaptureFactory
  {
    public StreamType CaptureType { get; set; }
    public ICapture Create()
    {
      switch (CaptureType)
      {
        case StreamType.Jpeg:
          return (new JpegStream());
        case StreamType.MJpeg:
          return (new MJpegStream());
        default:
          return (null);
      }
    }
  }
}
