namespace Vido.Parking
{
  using System;
  using System.Collections.Generic;

  public interface IUniqueIdDeviceList
  {
    #region Public Events
    /// <summary>
    /// Sự kiện được gọi khi có sự thay đổi danh sách thiết bị.
    /// </summary>
    event EventHandler DevicesChanged;
    #endregion

    #region Public Properties
    /// <summary>
    /// Danh sách thiết bị.
    /// </summary>
    ICollection<IUniqueIdDevice> Devices { get; }
    #endregion

    #region Public Methods
    /// <summary>
    /// Đăng kí thiết bị để nhận được thông báo khi có dữ liệu đến.
    /// </summary>
    /// <param name="deviceName">Tên thiết bị</param>
    /// <returns></returns>
    IUniqueIdDevice Register(string deviceName);

    /// <summary>
    /// Hủy đăng kí thiết bị.
    /// </summary>
    /// <param name="device"></param>
    void Unregister(IUniqueIdDevice device);
    #endregion
  }
}