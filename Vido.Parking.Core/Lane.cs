namespace Vido.Parking.Core
{
  using Vido.Capture.Interfaces;
  using Vido.Parking.Core.Interfaces;

  public class Lane : ILane
  {
    public Lane()
    {
    }

    public string Name { get; set; }
    public ICapture PlateCamera { get; set; }
    public ICapture FaceCamera { get; set; }
    public IUidDevice UidDevice { get; set; }
  }
}
