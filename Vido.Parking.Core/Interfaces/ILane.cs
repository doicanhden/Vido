namespace Vido.Parking.Core.Interfaces
{
  using Vido.Capture.Interfaces;

  public interface ILane
  {
    string Name { get; set; }
    ICapture PlateCamera { get; set; }
    ICapture FaceCamera { get; set; }
    IUidDevice UidDevice { get; set; }
  }
}
