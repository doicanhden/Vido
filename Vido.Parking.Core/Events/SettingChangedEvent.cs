namespace Vido.Parking.Events
{
  using System;

  public delegate void SettingChangedEventHandler(object sender, SettingChangedEventArgs e);

  public class SettingChangedEventArgs : EventArgs
  {
    #region Public Properties
    public string Key { get; private set; }
    #endregion

    #region Constructors
    public SettingChangedEventArgs(string key)
    {
      this.Key = key;
    }
    #endregion
  }
}
