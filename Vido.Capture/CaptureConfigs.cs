namespace Vido.Capture
{
  using Vido.Capture.Interfaces;

  public class CaptureConfigs : ICaptureConfigs
  {
    #region Public Properties
    public string Username { get; set; }
    public string Password { get; set; }
    public string Source { get; set; }
    public int FrameInterval { get; set; }
    #endregion

    #region Constructors
    public CaptureConfigs()
    {
    }
    #endregion
  }
}
