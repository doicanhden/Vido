namespace Vido.Parking.Interfaces
{
  using Vido.Parking.Events;

  /// <summary>
  /// Giao diện mô tả cách thức cung cấp thông tin cài đặt.
  /// </summary>
  public interface ISettingsProvider
  {
    event SettingChangedEventHandler SettingChanged;

    /// <summary>
    /// Tải thông tin từ nơi lưu trữ.
    /// </summary>
    void Load();

    /// <summary>
    /// Lưu thông tin vào nơi lưu trữ
    /// </summary>
    void Save();

    /// <summary>
    /// Thiết lập giá trị mới vào bộ lưu trữ.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của giá trị.</typeparam>
    /// <param name="key">Khóa</param>
    /// <param name="value">Giá trị</param>
    void Set<T>(string key, T value);

    /// <summary>
    /// Lấy giá trị từ bộ lưu trữ.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của giá trị cần lấy</typeparam>
    /// <param name="key">Khóa</param>
    /// <returns>Dữ liệu</returns>
    T Query<T>(string key);
  }
}
