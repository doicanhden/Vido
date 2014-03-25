using System;
using System.Collections.Generic;
using Vido.Parking.Events;
using Vido.Parking.Interfaces;
namespace Vido.Parking
{
  public class InputDeviceList : IUidDeviceList
  {
    #region Thread-Safe Singleton
    private static volatile InputDeviceList instance = null;
    private static object syncRoot = new Object();
    public static InputDeviceList GetInstance(IntPtr handle)
    {
      if (instance == null)
      {
        lock (syncRoot)
        {
          if (instance == null)
            instance = new InputDeviceList(handle);
        }
      }

      return (instance);
    }
    #endregion

    #region Data Members
    private readonly object objLock = new object();
    private readonly Dictionary<string, KeyDownBuffer> registered;
    private Vido.RawInput.RawInput rawInput = null;
    private List<IUidDevice> devices = null;
    #endregion

    #region Implementation of IUidDevicesEnumerator 
    public event DevicesChangedEventHandler DevicesChanged;

    public ICollection<IUidDevice> Devices
    {
      get
      {
        lock (objLock)
        {
          return (devices);
        }
      }
    }

    public IUidDevice Register(string deviceName)
    {
      if (!string.IsNullOrEmpty(deviceName))
      {
        var newDevice = new KeyDownBuffer() { Name = deviceName };
        registered[deviceName] = newDevice;

        return (newDevice);
      }

      return (null);
    }

    public void Unregister(IUidDevice device)
    {
      if (device != null)
      {
        if (registered.ContainsKey(device.Name))
        {
          registered.Remove(device.Name);
        }
      }
    }
    #endregion

    #region Private Constructors
    private InputDeviceList(IntPtr handle)
    {
      registered = new Dictionary<string, KeyDownBuffer>();

      rawInput = new Vido.RawInput.RawInput(handle);
      rawInput.Keyboard.DevicesChanged += Keyboard_DevicesChanged;
      rawInput.Keyboard.EnumerateDevices();
    }
    #endregion

    #region Event Handlers
    private void Keyboard_DevicesChanged(object sender, Vido.RawInput.Events.DevicesChangedEventArgs e)
    {
      var oldDevices = devices;
      if (e.OldDevices != null)
      {
        foreach (var keyboard in e.OldDevices)
        {
          keyboard.KeyDown -= keyboard_KeyDown;
        }
      }

      lock (objLock)
      {
        devices = null;

        if (e.NewDevices != null)
        {
          devices = new List<IUidDevice>();
          foreach (var keyboard in e.NewDevices)
          {
            devices.Add(new KeyDownBuffer() { Name = keyboard.Name });
            keyboard.KeyDown += keyboard_KeyDown;
          }
        }
      }

      if (DevicesChanged != null)
      {
        DevicesChanged(this, new DevicesChangedEventArgs(oldDevices, devices));
      }
    }

    private void keyboard_KeyDown(object sender, Vido.RawInput.Events.KeyEventArgs e)
    {
      var keyboard = sender as Vido.RawInput.Interfaces.IKeyboard;
      if (registered.ContainsKey(keyboard.Name))
      {
        registered[keyboard.Name].PushKey((byte)e.KeyValue);
      }
    }
    #endregion

    internal class KeyDownBuffer : IUidDevice
    {
      private readonly List<byte> buffer = new List<byte>();

      public event DataInEventHandler DataIn;

      public string Name { get; set; }

      internal void PushKey(byte data)
      {
        if (data == 13) // Enter key
        {
          if (DataIn != null)
          {
            DataIn(this, new DataInEventArgs(buffer.ToArray()));
          }

          buffer.Clear();
        }
        else
        {
          buffer.Add(data);
        }
      }
    }
  }
}
