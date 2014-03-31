namespace Vido.Parking
{
  using System;
  using Vido.Parking.Events;

  public interface IUidDevice
  {
    #region Public Events
    /// <summary>
    /// Sự kiện kích thoạt khi có dữ liệu đến từ thiết bị.
    /// </summary>
    event EventHandler DataIn;
    #endregion

    #region Public Properties
    /// <summary>
    /// Tên của thiết bị sinh dữ liệu Uid.
    /// </summary>
    string Name { get; set; }
    #endregion
  }
}