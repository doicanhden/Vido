namespace Vido.Parking.Models
{
  using Vido.Capture;
  using Vido.Parking.Enums;

  public class LaneConfigs
  {
    public CaptureConfigs FrontCamera { get; set; }
    public CaptureConfigs BackCamera  { get; set; }
    public string UidDeviceName { get; set; }
    public Direction Direction { get; set; }
    public LaneState State { get; set; }
    public int NumberOfRetries { get; set; }
  }
}
