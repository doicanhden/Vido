namespace Vido.Parking.Core.Interfaces
{
  public interface IUidDevice
  {
    string Name { get; set; }
    event UidEventHandler Uid;
  }
}
