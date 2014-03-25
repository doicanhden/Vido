namespace Vido.Parking.Events
{
  using System;
  using System.Collections.Generic;
  using Vido.Parking.Interfaces;

  public delegate void DevicesChangedEventHandler(object sender, DevicesChangedEventArgs e);

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

    #region Constructors
    public DevicesChangedEventArgs(ICollection<IUidDevice> oldDevices, ICollection<IUidDevice> newDevices)
    {
      this.OldDevices = oldDevices;
      this.NewDevices = newDevices;
    }
    #endregion
  }
}
