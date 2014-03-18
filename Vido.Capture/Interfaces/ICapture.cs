namespace Vido.Capture.Interfaces
{
  using System.Drawing;
  using Vido.Capture.Events;

  public interface ICapture
  {
    event NewFrameEventHandler NewFrame;
    int FramesReceived { get; }

    /// <summary>
    /// Open Capture using Source property
    /// </summary>
    /// <returns>true, if open successfully. Otherwise, return false</returns>
    bool Start();

    /// <summary>
    /// Close Capture
    /// </summary>
    void Stop();

    ICaptureConfigs Configs { get; set; }

    /// <summary>
    /// Take a image from Capture
    /// </summary>
    Image Take();
  }
}
