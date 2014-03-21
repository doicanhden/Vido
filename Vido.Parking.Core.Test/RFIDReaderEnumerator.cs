namespace Vido.Parking
{
  using System;
  using System.Collections.Generic;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;
  using Vido.RawInput;

  public class RFIDReaderEnumerator : IUidDevicesEnumerator
  {
    #region Thread-Safe Singleton
    private static volatile RFIDReaderEnumerator instance = null;
    private static object syncRoot = new Object();

    public static RFIDReaderEnumerator GetInstance(IntPtr handle)
    {
      if (instance == null)
      {
        lock (syncRoot)
        {
          if (instance == null)
            instance = new RFIDReaderEnumerator(handle);
        }
      }

      return (instance);
    }
    #endregion

    #region Data Members
    private readonly object objLock = new object();
    private RawInput rawInput = null;
    private List<IUidDevice> devices = null;
    #endregion

    #region Implementation of IUidDevicesEnumerator 
    public event DevicesChangedEventHandler DevicesChanged;
    public IList<IUidDevice> GetDevicesList()
    {
      lock (objLock)
      {
        return (devices);
      }
    }
    #endregion

    #region Private Constructors
    private RFIDReaderEnumerator(IntPtr handle)
    {
      rawInput = new RawInput(handle);
      rawInput.Keyboard.DevicesChanged += Keyboard_DevicesChanged;
    }
    #endregion

    #region Event Handlers
    private void Keyboard_DevicesChanged(object sender, Vido.RawInput.Events.DevicesChangedEventArgs e)
    {
      var oldDevices = devices;
      lock (objLock)
      {
        devices = new List<IUidDevice>();
        foreach (var keyboard in e.NewDevices)
        {
          devices.Add(new RFIDReader()
          {
            Name = keyboard.Name,
            Keyboard = keyboard
          });
        }
      }

      if (DevicesChanged != null)
      {
        DevicesChanged(this, new DevicesChangedEventArgs(oldDevices, devices));
      }
    }
    #endregion
  }
}
