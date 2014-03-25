
namespace Vido.Parking.Events
{
  using System;

  public delegate void MessageEventHandler(object sender, MessageEventArgs e);

  public class MessageEventArgs : EventArgs
  {
    public string Message { get; set; }

    public MessageEventArgs(string message)
    {
      this.Message = message;
    }
  }

}
