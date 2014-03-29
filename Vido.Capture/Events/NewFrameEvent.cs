namespace Vido.Capture.Events
{
  using System;
  using System.Drawing;

  /// <summary>
  /// Tham số cho sự kiện Khung hình mới trên thiết bị Chụp ảnh
  /// </summary>
  public class NewFrameEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Khung hình mới từ thiết bị Chụp ảnh
    /// </summary>
    public Bitmap Bitmap { get; private set; }
    #endregion

    #region Public Constructors
    public NewFrameEventArgs(Bitmap bitmap)
    {
      this.Bitmap = bitmap;
    }
    #endregion
  }
}