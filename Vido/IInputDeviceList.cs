namespace Vido
{
  using System;
  using System.Collections.Generic;

  public interface IInputDeviceList
  {
    #region Methods
    ICollection<IInputDevice> AllDevices();
    IDisposable Subscribe(IInputDevice device);
    #endregion
  }
}