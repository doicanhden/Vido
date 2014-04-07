namespace Vido.Desktop
{
  using System;
  using System.Collections.Generic;
  using Vido.Parking.Events;

  public class InputDeviceList : IInputDeviceList
  {
    #region Data Members
    private readonly object objLock = new object();
    private readonly List<IInputDevice> registered = null;
    private readonly RawInput.RawInput rawInput = null;
    #endregion

    #region Private Constructors
    public InputDeviceList(IntPtr handle)
    {
      registered = new List<IInputDevice>();

      rawInput = new Vido.RawInput.RawInput(handle);
      rawInput.AddMessageFilter();
      rawInput.Keyboard.DevicesChanged += Keyboard_DevicesChanged;
      rawInput.Keyboard.EnumerateDevices();
    }
    #endregion

    public ICollection<IInputDevice> AllDevices()
    {
      throw new NotImplementedException();
    }

    public IDisposable Subscribe(IInputDevice device)
    {
      if (!registered.Contains(device))
      {
        registered.Add(device);
      }

      return (new Unsubscriber(registered, device));
    }

    #region Event Handlers
    private void Keyboard_DevicesChanged(object sender, EventArgs e)
    {
      var args = e as RawInput.Events.DevicesChangedEventArgs;

      if (args.OldDevices != null)
      {
        foreach (var keyboard in args.OldDevices)
        {
          keyboard.KeyDown -= keyboard_KeyDown;
        }
      }

      if (args.NewDevices != null)
      {
        foreach (var keyboard in args.NewDevices)
        {
          keyboard.KeyDown += keyboard_KeyDown;
        }
      }
    }

    private void keyboard_KeyDown(object sender, EventArgs e)
    {
      var args = e as Vido.RawInput.Events.KeyEventArgs;
      var keyboard = sender as Vido.RawInput.IKeyboard;

      foreach (var reg in registered)
      {
        if (keyboard.Name.Contains(reg.Name))
        {
          (reg as InputDevice).Push((byte)args.KeyValue);
        }
      }
    }
    #endregion
  }

  private class Unsubscriber : IDisposable
  {
    private ICollection<IInputDevice> devices;
    private IInputDevice device;

    public Unsubscriber(ICollection<IInputDevice> devices, IInputDevice device)
    {
      this.devices = devices;
      this.device = device;
    }

    public void Dispose()
    {
      if (devices.Contains(device))
      {
        devices.Remove(device);
      }
    }
  }


}
