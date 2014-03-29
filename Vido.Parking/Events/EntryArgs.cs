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
    #endregion

    #region Public Constructors
    public EntryEventArgs(DataInEventArgs dataIn, string plateNumber, Image frontImage, Image backImage)
    {
      this.DataIn = dataIn;
      this.PlateNumber = plateNumber;
      this.FrontImage = frontImage;
      this.BackImage = backImage;
      this.Allow = true;
    }
    #endregion
  }
}