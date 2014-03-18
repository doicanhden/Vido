namespace Vido.RawInput.Interfaces
{
  using System;
  using Vido.RawInput.Events;

  public interface IKeyboard
  {
    event KeyEventHandler KeyUp;
    event KeyEventHandler KeyDown;

    string Description { get; set; }
    string Name { get; set; }
    string Type { get; set; }
    IntPtr Handle { get; set; }
  }
}
