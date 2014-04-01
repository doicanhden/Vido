namespace Vido.Parking
{
  using System;
  using System.Threading;
  using Vido.Capture;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;

  public class Lane
  {
    #region Data Members
    private IUniqueIdDevice uidDevice = null;
    #endregion

    #region Public Properties

    /// <summary>
    /// Mã Làn.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Trạng thái của Làn.
    /// </summary>
    public LaneState LaneState { get; set; }

    /// <summary>
    /// Hướng của Làn.
    /// </summary>
    public Direction Direction { get; set; }

    /// <summary>
    /// Số lần thử lại trước khi thông báo lỗi thiết bị.
    /// </summary>
    public int NumberOfRetries { get; set; }

    /// <summary>
    /// Thiết bị sinh dữ liệu Uid.
    /// </summary>
    public IUniqueIdDevice UidDevice
    {
      get { return (uidDevice); }
      set
      {
        if (uidDevice != null)
        {
          uidDevice.DataIn -= uidDevice_DataIn;
        }

        uidDevice = value;

        if (uidDevice != null)
        {
          uidDevice.DataIn += uidDevice_DataIn;
        }
      }
    }

    /// <summary>
    /// Camera Chụp ảnh Biển số phương tiện.
    /// </summary>
    public ICapture BackCamera { get; set; }

    /// <summary>
    /// Camera Chụp ảnh Người điều khiển.
    /// </summary>
    public ICapture FrontCamera { get; set; }
    #endregion

    #region Public Events
    /// <summary>
    /// Sự kiện kích hoạt khi phương tiện vào Làn.
    /// </summary>
    public event EventHandler Entry;

    /// <summary>
    /// Sự kiện kích hoạt khi phương tiện được phép di chuyển ra khỏi Làn.
    /// </summary>
    public event EventHandler EntryAllowed;

    /// <summary>
    /// Sự kiện kích hoạt khi có cập nhật mới về ảnh Biển số/Người điều khiển phương tiện.
    /// </summary>
    public event EventHandler SavedImages;

    /// <summary>
    /// Sự kiện kích hoạt khi có thông báo mời từ Làn.
    /// </summary>
    public event EventHandler NewMessage;
    #endregion

    #region Private Methods

    /// <summary>
    /// Kích hoạt sự kiện Thông báo mới với thời gian ở đầu thông báo.
    /// </summary>
    /// <param name="time">Thời gian</param>
    /// <param name="message">Thông báo</param>
    private void RaiseNewMessage(DateTime time, string message)
    {
      if (NewMessage != null)
      {
        NewMessage(this, new NewMessageEventArgs(time.ToString("HH:mm - ") + message));
      }
    }
    #endregion

    #region Event Handlers
    private void uidDevice_DataIn(object sender, EventArgs e)
    {
      var args = e as DataInEventArgs;

      if (args == null || LaneState == LaneState.Stop)
      {
        return;
      }

      if (Entry != null)
      {
        var entryTime = DateTime.Now;

        IImageHolder backImage = TryCapture(BackCamera);
        if (backImage != null)
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          RaiseNewMessage(entryTime, "Không thể chụp ảnh từ camera.");
          return;
        }

        IImageHolder frontImage = null;
        if (FrontCamera != null)
        {
          frontImage = TryCapture(FrontCamera);
          if (frontImage == null)
          {
            /// TODO: Địa phương hóa chuỗi thông báo.
            RaiseNewMessage(entryTime, "Không thể chụp ảnh từ camera.");
            return;
          }
        }

        var plateNumber = string.Empty;// Ocr.GetPlateNumber(backImage);
        var entryArgs = new EntryEventArgs(args, entryTime, plateNumber,
          backImage, frontImage);

        Entry(this, entryArgs);

        if (entryArgs.Allow)
        {
          RaiseNewMessage(entryTime, entryArgs.Message);

          if (SavedImages != null)
          {
            SavedImages(this, new SavedImagesEventArgs(
              entryArgs.BackImage, entryArgs.FrontImage));
          }

          if (EntryAllowed != null)
          {
            EntryAllowed(this, new EntryAllowedEventArgs(
              entryArgs.PlateNumber, entryArgs.Time));
          }
        }
        else
        {
          RaiseNewMessage(entryTime, entryArgs.Message);
        }
      }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Chụp ảnh từ Camera
    /// </summary>
    /// <param name="capture">Camera cần chụp ảnh.</param>
    /// <returns>Ảnh đã chụp từ camera.</returns>
    private IImageHolder TryCapture(ICapture capture)
    {
      if (capture != null)
      {
        for (int i = 0; i < NumberOfRetries; ++i)
        {
          var image = capture.Take();
          if (image != null && image.Available)
          {
            return (image);
          }
        }
      }

      return (null);
    }
    #endregion
  }
}