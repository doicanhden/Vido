namespace Vido.Capture
{
  using System;
  using System.Drawing;

  public interface ICapture : IDisposable
  {
    #region Public Events
    /// <summary>
    /// Sự kiện kích hoạt khi có khung hình mới từ thiết bị.
    /// </summary>
    event EventHandler NewFrame;

    /// <summary>
    /// Sự kiện kích hoại khi có lỗi xảy ra với thiết bị.
    /// </summary>
    event EventHandler ErrorOccurred;
    #endregion

    #region Public Properties
    /// <summary>
    /// Cấu hình thiết bị chụp ảnh.
    /// </summary>
    IConfigs Configs { get; set; }

    /// <summary>
    /// Số khung hình đã nhận được từ thiết bị.
    /// </summary>
    int FramesReceived { get; }
    #endregion

    #region Public Methods
    /// <summary>
    /// Open Capture using Source property
    /// </summary>
    /// <returns>true, if open successfully. Otherwise, return false</returns>
    bool Start();

    /// <summary>
    /// Close Capture
    /// </summary>
    void Stop();

    /// <summary>
    /// Take a image from Capture
    /// </summary>
    Image Take();
    #endregion
  }
}
