namespace Vido.Parking
{
  using System.Drawing;
  using Vido.Parking.Interfaces;
  public class Parking : IParking
  {
    public bool CanExit(byte[] data, string plateNumber)
    {
      return (true);
    }

    public void Exit(byte[] data, string plateNumber, Image frontImage, Image backImage)
    {
    }

    public bool CanEntry(byte[] data, string plateNumber)
    {
      return (true);
    }

    public void Entry(byte[] data, string plateNumber, Image frontImage, Image backImage)
    {
    }
  }
}
