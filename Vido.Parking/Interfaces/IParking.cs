namespace Vido.Parking.Interfaces
{
  public interface IParking
  {
    /// <summary>
    /// Thông tin cài đặt Bãi.
    /// </summary>
    ISettingsProvider Settings { get; }

    /// <summary>
    /// Kiểm tra xem phương tiện có thể Ra bãi hay không.
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Ra bãi, ngược lại: false</returns>
    bool CanOut(byte[] data, string plateNumber);

    /// <summary>
    /// Xử lý phương tiện Ra bãi.
    /// </summary>
    /// <param name="outArgs">Thông tin phương tiện ra.</param>
    void Out(InOutArgs outArgs);
    
    /// <summary>
    /// Kiểm tra xem phương tiện có thể Vào bãi hay không.
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Vào bãi, ngược lại: false</returns>
    bool CanIn(byte[] data, string plateNumber);

    /// <summary>
    /// Xử lý phương tiện Vào bãi.
    /// </summary>
    /// <param name="inArgs">Thông tin phương tiện vào bãi.</param>
    void In(InOutArgs inArgs);
  }
}
