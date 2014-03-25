namespace Vido.Parking.Interfaces
{
  using Vido.Parking.Events;

  public interface ISettingsProvider
  {
    event SettingChangedEventHandler SettingChanged;

    void Load();
    void Save();
    void Set<T>(string key, T value);
    T Query<T>(string key);
  }
}
