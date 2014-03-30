namespace Vido.Parking.Controls
{
  using System;
  using System.Drawing;
  using System.Threading;
  using Vido.Capture;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Utilities;

  public class Lane
  {
    #region Data Members
    private IUidDevice uidDevice = null;
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
    public IUidDevice UidDevice
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

        Image backImage = TryCapture(BackCamera);
        if (backImage == null)
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          RaiseNewMessage(entryTime, "Không thể chụp ảnh từ camera.");
          return;
        }

        Image frontImage = null;
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

        var plateNumber = Ocr.GetPlateNumber(backImage);
        var entryArgs = new EntryEventArgs(args, entryTime, plateNumber, backImage, frontImage);

        Entry(this, entryArgs);

        if (entryArgs.Allow)
        {
          if (Direction == Enums.Direction.In)
          {
            /// TODO: Địa phương hóa chuỗi thông báo.
            RaiseNewMessage(entryTime, "Mời xe VÀO bãi.");
          }
          else if (Direction == Enums.Direction.Out)
          {
            /// TODO: Địa phương hóa chuỗi thông báo.
            RaiseNewMessage(entryTime, "Mời xe RA bãi.");
          }

          if (SavedImages != null)
          {
            SavedImages(this, new SavedImagesEventArgs(frontImage, backImage));
          }

          if (EntryAllowed != null)
          {
            EntryAllowed(this, new EntryAllowedEventArgs(entryArgs.PlateNumber,
              entryArgs.Time));
          }
        }
        else
        {
          if (string.IsNullOrWhiteSpace(entryArgs.Message))
          {
            if (Direction == Enums.Direction.In)
            {
              /// TODO: Địa phương hóa chuỗi thông báo.
              RaiseNewMessage(entryTime, "Xe KHÔNG ĐƯỢC PHÉP VÀO bãi.");
            }
            else if (Direction == Enums.Direction.Out)
            {
              /// TODO: Địa phương hóa chuỗi thông báo.
              RaiseNewMessage(entryTime, "Xe KHÔNG ĐƯỢC PHÉP RA bãi.");
            }
          }
          else
          {
            RaiseNewMessage(entryTime, entryArgs.Message);
          }
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
    private Image TryCapture(ICapture capture)
    {
      if (capture != null)
      {
        for (int i = 0; i < NumberOfRetries; ++i)
        {
          // Chụp ảnh từ Camera.
          var image = capture.Take();
          if (image != null)
          {
            return (image);
          }

          // Chờ 0.15s cho lần chụp kế tiếp.
          Thread.Sleep(150);
        }
      }

      return (null);
    }
    #endregion
  }
}