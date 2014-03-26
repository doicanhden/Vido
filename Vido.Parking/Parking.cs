namespace Vido.Parking
{
  using System;
  using System.Drawing;
  using System.IO;
  using Vido.Parking.Interfaces;

  public class Parking : IParking, IDisposable
  {
    private VidoParkingEntities entities = new VidoParkingEntities();

    /// <summary>
    /// Thông tin cài đặt Bãi.
    /// </summary>
    public ISettingsProvider Settings { get; set; }

    /// <summary>
    /// Kiểm tra xem phương tiện có thể Ra bãi hay không.
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Ra bãi, ngược lại: false</returns>
    public bool CanOut(byte[] data, string plateNumber)
    {
      return (true);
    }

    /// <summary>
    /// Kiểm tra xem phương tiện có thể Vào bãi hay không.
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Vào bãi, ngược lại: false</returns>
    public bool CanIn(byte[] data, string plateNumber)
    {
      // TODO: Trả về vị trí Phương tiện có thể Đỗ.
      return (true);
    }

    /// <summary>
    /// Xử lý phương tiện Ra bãi.
    /// </summary>
    /// <param name="outArgs">Thông tin phương tiện ra.</param>
    public void Out(InOutArgs outArgs)
    {
      // TODO: Update into DB.
    }

    /// <summary>
    /// Xử lý phương tiện Vào bãi.
    /// </summary>
    /// <param name="inArgs">Thông tin phương tiện vào bãi.</param>
    public void In(InOutArgs inArgs)
    {
      // TODO: Update into DB.
    }

    #region Implementation of IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose managed resources
        entities.Dispose();
      }
      // free native resources
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion
  }
}
