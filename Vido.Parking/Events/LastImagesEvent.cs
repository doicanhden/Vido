namespace Vido.Parking.Events
{
  using System;
  using System.Drawing;

  public delegate void LastImagesEventHandler(object sender, LastImagesEventArgs e);

  public class LastImagesEventArgs : EventArgs
  {
    #region Public Properties
    public Image Front { get; set; }
    public Image Back { get; set; }
    #endregion

    #region Constructors
    public LastImagesEventArgs(Image front, Image back)
    {
      this.Front = front;
      this.Back = back;
    }
    #endregion
  }
}
