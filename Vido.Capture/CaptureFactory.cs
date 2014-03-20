namespace Vido.Capture
{
  using Vido.Capture.Enums;
  using Vido.Capture.Interfaces;
  public class CaptureFactory : ICaptureFactory
  {
    /// <summary>
    /// Tạo đối tượng Capture và set Config.
    /// </summary>
    /// <param name="configs"></param>
    /// <returns></returns>
    public ICapture Create(CaptureConfigs configs)
    {
      ICapture capture = null;
      switch (configs.Coding)
      {
        case Coding.Jpeg:
          capture = new JpegStream();
          break;
        case Coding.MJpeg:
          capture = new MJpegStream();
          break;
      }

      if (capture != null)
      {
        capture.Configs = configs;
      }

      return (capture);
    }
  }
}
