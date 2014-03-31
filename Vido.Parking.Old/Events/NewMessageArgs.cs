namespace Vido.Parking.Events
{
  using System;

  public class NewMessageEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Thông báo.
    /// </summary>
    public string Message { get; set; }
    #endregion

    #region Public Constructors
    public NewMessageEventArgs(string message)
    {
      this.Message = message;
    }
    #endregion
  }
}