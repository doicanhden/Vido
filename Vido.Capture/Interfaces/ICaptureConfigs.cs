using Vido.Capture.Enums;
namespace Vido.Capture.Interfaces
{
  public interface ICaptureConfigs
  {
    /// <summary>
    /// Gets or sets username for login.
    /// </summary>
    string Username { get; set; }

    /// <summary>
    /// Gets or sets password for login.
    /// </summary>
    string Password { get; set; }

    /// <summary>
    /// Gets or sets Source for Capture.
    /// </summary>
    string Source { get; set; }

    int FrameInterval { get; set;}

    Coding Coding { get; set; }
  }
}
