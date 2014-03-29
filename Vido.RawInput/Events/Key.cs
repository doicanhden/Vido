namespace Vido.RawInput.Events
{
  using System;
  using Vido.RawInput;

  public delegate void KeyEventHandler(object sender, KeyEventArgs e);

  public class KeyEventArgs : EventArgs
  {
    private int keyValue;

    public KeyEventArgs(int keyCode)
    {
      this.keyValue = keyCode;
    }
    public int KeyValue
    {
      get { return (keyValue); }
    }
  }
}
