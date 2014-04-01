namespace Vido.Parking.Events
{
  using System;

  public class SavedImagesEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Ảnh chụp Biển số phương tiện.
    /// </summary>
    public IImageHolder Back { get; set; }

    /// <summary>
    /// Ảnh chụp Người điều khiển phương tiện.
    /// </summary>
    public IImageHolder Front { get; set; }
    #endregion

    #region Public Constructors
    public SavedImagesEventArgs(IImageHolder back, IImageHolder front)
    {
      this.Back = back;
      this.Front = front;
    }
    #endregion
  }
}