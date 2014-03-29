namespace Vido.Parking.Events
{
  using System;
  using System.Drawing;

  public class SavedImagesEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Ảnh chụp Biển số phương tiện.
    /// </summary>
    public Image Back { get; set; }

    /// <summary>
    /// Ảnh chụp Người điều khiển phương tiện.
    /// </summary>
    public Image Front { get; set; }
    #endregion

    #region Public Constructors
    public SavedImagesEventArgs(Image front, Image back)
    {
      this.Front = front;
      this.Back = back;
    }
    #endregion
  }
}