using System;
namespace Vido.Parking.Events
{

  public delegate void SettingChangedEventHandler(object sender, SettingChangedEventArgs e);

  public class SettingChangedEventArgs : EventArgs
  {
    #region Public Properties
    public string SettingKey { get; private set; }
    #endregion

    #region Constructors
    public SettingChangedEventArgs(string settingKey)
    {
      this.SettingKey = settingKey;
    }
    #endregion
  }
}
