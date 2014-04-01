namespace Vido.Parking
{
  using System;
  using Vido.Capture;
  using Vido.Parking.Enums;

  public interface ILane
  {
    #region Public Properties

    /// <summary>
    /// Mã Làn.
    /// </summary>
    string Code { get; set; }

    /// <summary>
    /// Trạng thái của Làn.
    /// </summary>
    LaneState LaneState { get; set; }

    /// <summary>
    /// Hướng của Làn.
    /// </summary>
    Direction Direction { get; set; }

    /// <summary>
    /// Số lần thử lại trước khi thông báo lỗi thiết bị.
    /// </summary>
    int NumberOfRetries { get; set; }

    /// <summary>
    /// Thiết bị sinh dữ liệu Uid.
    /// </summary>
    IUniqueIdDevice UidDevice { get; set; }

    /// <summary>
    /// Camera Chụp ảnh Biển số phương tiện.
    /// </summary>
    ICapture BackCamera { get; set; }

    /// <summary>
    /// Camera Chụp ảnh Người điều khiển.
    /// </summary>
    ICapture FrontCamera { get; set; }
    #endregion

    #region Public Events
    /// <summary>
    /// Sự kiện kích hoạt khi phương tiện vào Làn.
    /// </summary>
    event EventHandler Entry;

    /// <summary>
    /// Sự kiện kích hoạt khi phương tiện được phép di chuyển ra khỏi Làn.
    /// </summary>
    event EventHandler EntryAllowed;

    /// <summary>
    /// Sự kiện kích hoạt khi có cập nhật mới về ảnh Biển số/Người điều khiển phương tiện.
    /// </summary>
    event EventHandler SavedImages;

    /// <summary>
    /// Sự kiện kích hoạt khi có thông báo mời từ Làn.
    /// </summary>
    event EventHandler NewMessage;
    #endregion
  }
}
