namespace Vido.Parking.Interfaces
{
  using Vido.Parking.Events;

  public interface IUidDevice
  {
    /// <summary>
    /// Sự kiện kích thoạt khi có dữ liệu đến từ thiết bị.
    /// </summary>
    event DataInEventHandler DataIn;

    /// <summary>
    /// Tên của thiết bị sinh dữ liệu Uid.
    /// </summary>
    string Name { get; set; }
  }
}
