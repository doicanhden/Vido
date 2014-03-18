namespace Vido.RawInput.Interfaces
{
  using System.Collections.Generic;
  using Vido.RawInput.Events;

  public interface IRawKeyboard
  {
    event DevicesChangedEventHandler DevicesChanged;
    IList<IKeyboard> Devices { get; }

    int EnumerateDevices();
  }
}
