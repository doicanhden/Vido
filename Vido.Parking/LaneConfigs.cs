namespace Vido.Parking
{
  using System;
  using Vido.Capture;
  using Vido.Parking.Enums;

  public class LaneConfigs
  {
    #region Public Properties
    /// <summary>
    /// Cài đặt Camera chụp ảnh Người điều khiển phương tiện.
    /// </summary>
    public Configs FrontCamera { get; set; }

    /// <summary>
    /// Cài đặt Camera chụp ảnh Biển số phương tiện.
    /// </summary>
    public Configs BackCamera  { get; set; }

    /// <summary>
    /// Tên của thiết bị sinh Uid.
    /// </summary>
    public string UidDeviceName { get; set; }

    /// <summary>
    /// Hướng di chuyển của Làn (VÀO/RA Bãi).
    /// </summary>
    public Direction Direction { get; set; }

    /// <summary>
    /// Trạng thái của Làn (Ready/Stop).
    /// </summary>
    public LaneState State { get; set; }

    /// <summary>
    /// Số lần thử lại trước khi thông báo lỗi thiết bị.
    /// </summary>
    public int NumberOfRetries { get; set; }
    #endregion
  }
}