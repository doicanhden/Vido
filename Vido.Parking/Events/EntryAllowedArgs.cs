namespace Vido.Parking.Events
{
  using System;

  public class EntryAllowedEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Biển số phương tiện
    /// </summary>
    public string PlateNumber { get; set; }
    public DateTime Time { get; set; }
    #endregion

    #region Public Constructors
    public EntryAllowedEventArgs(string plateNumber, DateTime time)
    {
      this.PlateNumber = plateNumber;
      this.Time = time;
    }
    #endregion
  }
}