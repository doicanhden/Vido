namespace Vido.RawInput
{
  using System;
  using Vido.RawInput.Events;
  using Vido.RawInput.Interfaces;

  public class Keyboard : IKeyboard
  {
    #region Events
    public event KeyEventHandler KeyUp;
    public event KeyEventHandler KeyDown;
    #endregion

    #region Properties
    public IntPtr Handle { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    #endregion

    public Keyboard()
    {
    }

    #region Methods
    internal void RaiseKeyDown(int keyValue)
    {
      if (KeyDown != null)
      {
        KeyDown(this, new KeyEventArgs(keyValue));
      }
    }
    internal void RaiseKeyUp(int keyValue)
    {
      if (KeyUp != null)
      {
        KeyUp(this, new KeyEventArgs(keyValue));
      }
    }
    #endregion
  }
}
