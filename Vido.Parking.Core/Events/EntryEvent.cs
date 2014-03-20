namespace Vido.Parking.Events
{
  using System;
  using System.Drawing;

  public delegate void EntryEventHandler(object s, EntryEventArgs e);

  public class EntryEventArgs : EventArgs
  {
    #region Public Properties
    public byte[] Uid { get; private set; }
    public Image FrontImage { get; private set; }
    public Image BackImage { get; private set; }
    #endregion

    #region Constructors
    public EntryEventArgs(byte[] uid, Image frontImage, Image backImage)
    {
      this.Uid = uid;
      this.FrontImage = frontImage;
      this.BackImage = backImage;
    }
    #endregion
  }
}
