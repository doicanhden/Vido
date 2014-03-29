namespace Vido.Parking.Events
{
  using System;

  public class SettingChangedEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Khóa của Cài đặt.
    /// </summary>
    public string SettingKey { get; private set; }
    #endregion

    #region Public Constructors
    public SettingChangedEventArgs(string settingKey)
    {
      this.SettingKey = settingKey;
    }
    #endregion
  }
}
