namespace Vido.Capture
{
  using Vido.Capture.Enums;

  public interface IConfigs
  {
    #region Public Properties
    /// <summary>
    /// Tên đăng nhập thiết bị
    /// </summary>
    string Username { get; set; }

    /// <summary>
    /// Mật khẩu đăng nhập thiết bị.
    /// </summary>
    string Password { get; set; }

    /// <summary>
    /// Nguồn cung cấp khung hình.
    /// </summary>
    string Source { get; set; }

    /// <summary>
    /// Định dạng nén dữ liệu Video.
    /// </summary>
    Coding Coding { get; set; }

    /// <summary>
    /// Thời gian giữa mỗi khung hình.
    /// </summary>
    int FrameInterval { get; set;}
    #endregion
  }
}