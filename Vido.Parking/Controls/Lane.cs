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

    #region Public Methods
    /// <summary>
    /// Kích hoạt sự kiện Thông báo mới.
    /// </summary>
    /// <param name="message">Thông báo</param>
    public void RaiseNewMessage(string message)
    {
      if (NewMessage != null)
      {
        NewMessage(this, new NewMessageEventArgs(message));
      }
    }
    #endregion

    #region Event Handlers
    private void uidDevice_DataIn(object sender, EventArgs e)
    {
      var args = e as DataInEventArgs;

      if (args.Data == null || Entry == null || LaneState == LaneState.Stop)
      {
        return;
      }

      Image backImage = TryCapture(BackCamera);
      if (backImage == null)
      {
        RaiseNewMessage("Không thể chụp ảnh từ camera. Vui lòng kiểm tra thiết bị");
        return;
      }

      Image frontImage = null;
      // Có thể không thiết lập camera chụp Người điều khiển.
      if (FrontCamera != null)
      {
        frontImage = TryCapture(FrontCamera);
        if (frontImage == null)
        {
          RaiseNewMessage("Không thể chụp ảnh từ camera. Vui lòng kiểm tra thiết bị");
          return;
        }
      }

      // TODO: Kiểm tra xem có thể dùng Thread ở đây không.
      // Do tốn nhiều thời gian xử lý OCR để lấy biển số.

      var plateNumber = Ocr.GetPlateNumber(frontImage);
      var entryArg = new EntryEventArgs(args, plateNumber, frontImage, backImage);

      Entry(this, entryArg);

      if (entryArg.Allow)
      {
        if (EntryAllowed != null)
        {
          EntryAllowed(this, new EntryAllowedEventArgs(entryArg.PlateNumber));
        }

        if (SavedImages != null)
        {
          SavedImages(this, new SavedImagesEventArgs(frontImage, backImage));
        }
      }
      else
      {
        // TODO: Fix Hard-Code.
        var message = string.Format("Phương tiện {0}, KHÔNG ĐƯỢC PHÉP ", plateNumber);

        if (Direction == Enums.Direction.In)
        {
          RaiseNewMessage(message + " VÀO bãi.");
        }
        else if (Direction == Enums.Direction.Out)
        {
          RaiseNewMessage(message + " RA bãi.");
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

          // Chờ 0.25s cho lần chụp kế tiếp.
          Thread.Sleep(250);
        }
      }

      return (null);
    }
    #endregion
  }
}