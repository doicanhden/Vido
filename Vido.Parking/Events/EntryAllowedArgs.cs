namespace Vido.Parking.Events
{
  using System;
  using System.Drawing;

  public class EntryAllowedEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Biển số phương tiện
    /// </summary>
    public string PlateNumber { get; set; }
    #endregion

    #region Public Constructors
    public EntryAllowedEventArgs(string plateNumber)
    {
      this.PlateNumber = plateNumber;
    }
    #endregion
  }
}