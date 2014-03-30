namespace Vido.Parking.Events
{
  using System;
  using System.Drawing;

  public class EntryEventArgs : EventArgs
  {
    #region Public Properties

    /// <summary>
    /// Dữ liệu từ Thiết bị Uid.
    /// </summary>
    public DataInEventArgs DataIn { get; private set; }

    /// <summary>
    /// Thời gian phương tiện vào làn.
    /// </summary>
    public DateTime Time { get; private set; }

    /// <summary>
    /// Biển số phương tiện.
    /// </summary>
    public string PlateNumber { get; private set; }

    /// <summary>
    /// Ảnh chụp Biển số phương tiện.
    /// </summary>
    public Image BackImage { get; set; }

    /// <summary>
    /// Ảnh chụp Người điều khiển phương tiện.
    /// </summary>
    public Image FrontImage { get; set; }

    /// <summary>
    /// Cho phép phương tiện ra khỏi làn.
    /// </summary>
    public bool Allow { get; set; }

    /// <summary>
    /// Thông báo
    /// </summary>
    public string Message { get; set; }
    #endregion

    #region Public Constructors
    public EntryEventArgs(DataInEventArgs dataIn, DateTime time,
      string plateNumber, Image backImage, Image frontImage)
    {
      this.DataIn = dataIn;
      this.Time = time;
      this.PlateNumber = plateNumber;
      this.BackImage = backImage;
      this.FrontImage = frontImage;
      this.Allow = false;
    }
    #endregion
  }
}