namespace Vido.Parking.Interfaces
{
  using System.Drawing;
/// <summary>go vo
/// 
/// </summary>
  public interface IParking
  {
    ISettingsProvider Settings { get; }

    bool CanExit(byte[] data, string plateNumber);
    void Exit(byte[] data, string plateNumber, Image frontImage, Image backImage);

    bool CanEntry(byte[] data, string plateNumber);
    void Entry(byte[] data, string plateNumber, Image frontImage, Image backImage);
  }
}
