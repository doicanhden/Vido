namespace Vido.Parking.Utilities
{
  using System;
  /// <summary>
  /// Lớp chứa công cụ. Không thể kế thừa.
  /// </summary>
  public static class Encode
  {
    /// <summary>
    /// Mã hóa dữ liệu Uid thành dạng có thể In
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <returns>Chuỗi có thể In</returns>
    public static string EncodeData(byte[] data)
    {
      return (Convert.ToBase64String(data));
    }
  }
}
