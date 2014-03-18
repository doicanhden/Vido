namespace Vido.Parking.Core
{
  using System.Text;
  using System.Xml.Serialization;
  using Vido.Parking.Core.Interfaces;
  using Vido.RawInput.Events;
  using Vido.RawInput.Interfaces;

  public class RFIDReader : IUidDevice
  {
    #region Data Members
    private readonly StringBuilder buffer = new StringBuilder();
    private IKeyboard keyboard = null;
    #endregion

    #region Events
    public event UidEventHandler Uid;
    #endregion

    public RFIDReader()
    {
    }

    #region Public Properties
    public string Name { get; set; }

    [XmlIgnore]
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
    private void keyboard_KeyDown(IKeyboard s, KeyEventArgs e)
    {
      if (e.KeyValue == 13) // Enter key
      {
        if (Uid != null)
        {
          Uid(this, new UidEventArgs(buffer.ToString()));
        }

        buffer.Clear();
      }
      else
      {
        buffer.Append(System.Convert.ToChar(e.KeyValue));
      }
    }
    #endregion

  }
}
