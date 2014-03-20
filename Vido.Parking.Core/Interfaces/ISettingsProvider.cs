namespace Vido.Parking.Interfaces
{
  public interface ISettingsProvider
  {
    void Load();
    void Save();

    T Query<T>(string key);
    void Set<T>(string key, T value);
  }
}
