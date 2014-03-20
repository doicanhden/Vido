namespace Vido.Parking.Events
{
  using System;
  using System.Collections.Generic;
  using Vido.Parking.Interfaces;

  public delegate void DevicesChangedEventHandler(object s, DevicesChangedEventArgs e);

  public class DevicesChangedEventArgs : EventArgs
  {
    #region Public Properties
    public IList<IUidDevice> OldDevices { get; private set; }
    public IList<IUidDevice> NewDevices { get; private set; }
    #endregion

    #region Constructors
    public DevicesChangedEventArgs(IList<IUidDevice> oldDevices, IList<IUidDevice> newDevices)
    {
      this.OldDevices = oldDevices;
      this.NewDevices = newDevices;
    }
    #endregion
  }
}
