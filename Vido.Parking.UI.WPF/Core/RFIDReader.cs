namespace Vido.Parking
{
  using System.Collections.Generic;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;
  using Vido.RawInput.Events;
  using Vido.RawInput.Interfaces;

  public class RFIDReader : IUidDevice
  {
    #region Data Members
    private readonly List<byte> buffer = new List<byte>();
    private IKeyboard keyboard = null;
    #endregion

    #region Events
    public event DataInEventHandler DataIn;
    #endregion

    #region Public Properties
    public string Name { get; set; }

    public IKeyboard Keyboard
    {
      get { return (keyboard); }
      set
      {
        if (keyboard != null)
        {
          keyboard.KeyDown -= keyboard_KeyDown;
        }

        keyboard = value;

        if (keyboard != null)
        {
          keyboard.KeyDown += keyboard_KeyDown;
        }
      }
    }
    #endregion

    #region Private Methods
    private void keyboard_KeyDown(object s, KeyEventArgs e)
    {
      if (e.KeyValue == 13) // Enter key
      {
        if (DataIn != null)
        {
          DataIn(this, new DataInEventArgs(buffer.ToArray()));
        }

        buffer.Clear();
      }
      else
      {
        buffer.Add((byte)e.KeyValue);
      }
    }
    #endregion
  }
}
