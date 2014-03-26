using System;
using System.Drawing;
using System.Threading;
using Vido.Capture.Interfaces;
using Vido.Parking.Enums;
using Vido.Parking.Events;
using Vido.Parking.Interfaces;
using Vido.Parking.Utilities;
namespace Vido.Parking.Controls
{
  public class Lane
  {
    #region Data Members
    private IUidDevice uidDevice = null;
    #endregion

    #region Public Properties

    /// <summary>
    /// Mã Làn.
    /// </summary>
    public string LaneCode { get; set; }

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
    public ICapture BackCamera  { get; set; }

    /// <summary>
    /// Camera Chụp ảnh Người điều khiển.
    /// </summary>
    public ICapture FrontCamera { get; set; }
    #endregion

    #region Public Events
    /// <summary>
    /// Sự kiện kích hoạt khi phương tiện vào Làn.
    /// </summary>
    public event EntryEventHandler Entry;

    /// <summary>
    /// Sự kiện kích hoạt khi phương tiện được phép di chuyển ra khỏi Làn.
    /// </summary>
    public event EntryAllowedEventHandler EntryAllowed;

    /// <summary>
    /// Sự kiện kích hoạt khi có cập nhật mới về ảnh Biển số/Người điều khiển phương tiện.
    /// </summary>
    public event LastImagesEventHandler LastImages;

    /// <summary>
    /// Sự kiện kích hoạt khi có thông báo mời từ Làn.
    /// </summary>
    public event MessageEventHandler NewMessage;
    #endregion

    #region Event Handlers

    private void uidDevice_DataIn(object sender, DataInEventArgs e)
    {
      if (e.Data == null || Entry == null || LaneState == LaneState.Stop)
      {
        return;
      }

      try
      {
        Image backImage = TryCapture(BackCamera);
        Image frontImage = null;

        // Có thể không thiết lập camera chụp Người điều khiển.
        if (FrontCamera != null)
        {
          frontImage = TryCapture(FrontCamera);
        }

        // TODO: Kiểm tra xem có thể dùng Thread ở đây không.
        // Do tốn nhiều thời gian xử lý OCR để lấy biển số.

//      Task task = new Task(() =>
//      {
          var plateNumber = Ocr.GetPlateNumber(frontImage);
          var entryArg = new EntryEventArgs(e.Data, plateNumber, frontImage, backImage);

          Entry(this, entryArg);

          if (entryArg.Allow)
          {
            if (EntryAllowed != null)
            {
              EntryAllowed(this, new EntryAllowedEventArgs(entryArg.PlateNumber));
            }

            if (LastImages != null)
            {
              LastImages(this, new LastImagesEventArgs(frontImage, backImage));
            }
          }
          else
          {
            if (NewMessage != null)
            {
              // TODO: Fix Hard-Code.
              var message = string.Format("Phương tiện {0}, KHÔNG ĐƯỢC PHÉP ", plateNumber);
              if (Direction == Enums.Direction.In)
              {
                NewMessage(this, new NewMessageEventArgs(message + " VÀO bãi."));
              }
              else if (Direction == Enums.Direction.Out)
              {
                NewMessage(this, new NewMessageEventArgs(message + " RA bãi."));
              }
            }
          }
//      });

//      task.Start();
      }
      catch(InvalidOperationException)
      {
        if (NewMessage != null)
        {
          // TODO: Fix Hard-Code.
          NewMessage(this, new NewMessageEventArgs(
            @"Không thể chụp ảnh từ camera.
            Vui lòng kiểm tra thiết bị"));
        }
        throw;
      }
    }
    #endregion

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

      throw new InvalidOperationException("Can't capture from Device");
    }
  }
}
