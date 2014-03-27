using System;
using System.Collections.Generic;
using Vido.Parking.Events;
using Vido.Parking.Interfaces;
namespace Vido.Parking
{
  class KeyDownBuffer : IUidDevice
  {
    #region Data Members
    private readonly List<byte> buffer = new List<byte>();
    #endregion

    #region Public Events
    public event DataInEventHandler DataIn;
    #endregion

    #region Public Properties
    public string Name { get; set; }
    public byte EndKey { get; set; }
    #endregion

    #region Public Methods
    /// <summary>
    /// Thêm một kí tự vào bộ đệm,
    /// kích hoạt sự kiện DataIn khi gặp EndKey
    /// </summary>
    /// <param name="data">Mã ASCII của phím.</param>
    public void PushKey(byte data)
    {
      if (data == EndKey)
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
    #endregion
  }

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
    private readonly List<KeyDownBuffer> registered = null;
    private RawInput.RawInput rawInput = null;
    private List<IUidDevice> devices = null;
    #endregion

    #region Private Constructors
    private InputDeviceList(IntPtr handle)
    {
      registered = new List<KeyDownBuffer>();

      rawInput = new Vido.RawInput.RawInput(handle);
      rawInput.AddMessageFilter();
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

      foreach (var reg in registered)
      {
        if (keyboard.Name.Contains(reg.Name))
        {
          reg.PushKey((byte)e.KeyValue);
        }
      }
    }
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
      return (Register(deviceName, 13)); // 13 - Enter Key.
    }

    public IUidDevice Register(string deviceName, byte endKey)
    {
      if (!string.IsNullOrEmpty(deviceName))
      {
        var newDevice = new KeyDownBuffer()
        {
          Name = deviceName,
          EndKey = endKey // Enter Key.
        };
        registered.Add(newDevice);

        return (newDevice);
      }

      return (null);
    }

    public void Unregister(IUidDevice device)
    {
      if (device != null)
      {
        registered.RemoveAll((x) => x.Name == device.Name);
      }
    }
    #endregion
  }
}
