namespace Vido.Parking.Events
{
  using System;
  using System.Drawing;

  public delegate void EntryEventHandler(object sender, EntryEventArgs e);

  public class EntryEventArgs : EventArgs
  {
    #region Public Properties
    public byte[] Data { get; private set; }
    public Image FrontImage { get; private set; }
    public Image BackImage { get; private set; }
    public bool Allow { get; set; }
    public string PlateNumber { get; set; }
    #endregion

    #region Constructors
    public EntryEventArgs(byte[] uid, string plateNumber, Image frontImage, Image backImage)
    {
      this.Data = uid;
      this.PlateNumber = plateNumber;
      this.FrontImage = frontImage;
      this.BackImage = backImage;
      this.Allow = true;
    }
    #endregion
  }
}
