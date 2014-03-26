
namespace Vido.Parking.Events
{
  using System;

  public delegate void MessageEventHandler(object sender, NewMessageEventArgs e);

  public class NewMessageEventArgs : EventArgs
  {
    public string Message { get; set; }

    public NewMessageEventArgs(string message)
    {
      this.Message = message;
    }
  }

}
