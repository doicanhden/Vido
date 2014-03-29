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
    public ICollection<IUidDevice> OldDevices { get; private set; }

    /// <summary>
    /// Danh sách thiết bị mới.
    /// </summary>
    public ICollection<IUidDevice> NewDevices { get; private set; }
    #endregion

    #region Public Constructors
    public DevicesChangedEventArgs(ICollection<IUidDevice> oldDevices, ICollection<IUidDevice> newDevices)
    {
      this.OldDevices = oldDevices;
      this.NewDevices = newDevices;
    }
    #endregion
  }
}