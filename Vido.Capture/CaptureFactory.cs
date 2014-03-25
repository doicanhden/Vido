﻿namespace Vido.Capture
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
    public virtual ICapture Create(ICaptureConfigs configs)
    {
      ICapture capture = null;
      if (configs != null)
      {
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
      }

      return (capture);
    }
  }
}
