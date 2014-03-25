﻿namespace Vido.Parking.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Vido.Parking.Events;

  public interface IUidDeviceList
  {
    /// <summary>
    /// Sự kiện được gọi khi có sự thay đổi danh sách thiết bị.
    /// </summary>
    event DevicesChangedEventHandler DevicesChanged;

    /// <summary>
    /// Danh sách thiết bị.
    /// </summary>
    ICollection<IUidDevice> Devices { get; }

    /// <summary>
    /// Đăng kí thiết bị để nhận được thông báo khi có dữ liệu đến.
    /// </summary>
    /// <param name="deviceName">Tên thiết bị</param>
    /// <returns></returns>
    IUidDevice Register(string deviceName);

    /// <summary>
    /// Hủy đăng kí thiết bị.
    /// </summary>
    /// <param name="device"></param>
    void Unregister(IUidDevice device);
  }
}
