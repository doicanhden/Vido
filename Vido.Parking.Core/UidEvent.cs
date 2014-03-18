using System;
namespace Vido.Parking.Core
{
  public delegate void UidEventHandler(object s, UidEventArgs e);

  public class UidEventArgs : EventArgs
  {
    public string Uid { get; private set; }

    public UidEventArgs(string uid)
    {
      this.Uid = uid;
    }
  }
}
