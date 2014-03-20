namespace Vido.Parking.Interfaces
{
  using Vido.Parking.Events;

  public interface IUidDevice
  {
    event DataInEventHandler DataIn;

    /// <summary>
    /// Gets or sets Name of Uid device
    /// </summary>
    string Name { get; set; }
  }
}
