namespace Vido
{
  using System;

  public interface IInputDevice
  {
    #region Public Events
    event EventHandler DataIn;
    #endregion

    #region Public Properties
    string Name { get; set; }
    #endregion
  }
}