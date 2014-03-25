namespace Vido.RawInput.Interfaces
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
