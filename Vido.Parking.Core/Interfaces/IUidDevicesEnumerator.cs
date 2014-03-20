namespace Vido.Parking.Interfaces
{
  using System;
  using System.Collections.Generic;
  using Vido.Parking.Events;

  public interface IUidDevicesEnumerator
  {
    event DevicesChangedEventHandler DevicesChanged;
    IList<IUidDevice> GetDevicesList();
  }
}
