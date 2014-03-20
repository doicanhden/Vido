namespace Vido.Parking.Interfaces
{
  using System.Drawing;

  public interface IParking
  {
    bool CanExit(byte[] data, string plateNumber);
    void Exit(byte[] data, string plateNumber, Image frontImage, Image backImage);

    bool CanEntry(byte[] data, string plateNumber);
    void Entry(byte[] data, string plateNumber, Image frontImage, Image backImage);
  }
}
