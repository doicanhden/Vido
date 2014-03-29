namespace Vido.RawInput
{
  using System.Collections.Generic;
  using Vido.RawInput.Events;

  public interface IRawKeyboard
  {
    event DevicesChangedEventHandler DevicesChanged;
    ICollection<IKeyboard> Devices { get; }

    int EnumerateDevices();
  }
}
