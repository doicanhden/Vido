namespace Vido.Parking.Interfaces
{
  using Vido.Parking.Events;

  public interface ISettingsProvider
  {
    event SettingChangedEventHandler SettingChanged;

    void Load();
    void Save();

    T Query<T>(string key);
    void Set<T>(string key, T value);
  }
}
