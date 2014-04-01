namespace Vido.Parking.Events
{
  using System;
  using System.Collections.Generic;

  public class DevicesChangedEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Danh sách thiết bị cũ.
    /// </summary>
    public ICollection<IUniqueIdDevice> OldDevices { get; private set; }

    /// <summary>
    /// Danh sách thiết bị mới.
    /// </summary>
    public ICollection<IUniqueIdDevice> NewDevices { get; private set; }
    #endregion

    #region Public Constructors
    public DevicesChangedEventArgs(ICollection<IUniqueIdDevice> oldDevices, ICollection<IUniqueIdDevice> newDevices)
    {
      this.OldDevices = oldDevices;
      this.NewDevices = newDevices;
    }
    #endregion
  }
}