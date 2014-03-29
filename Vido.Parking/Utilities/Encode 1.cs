namespace Vido.Parking.Utilities
{
  using System;
  using System.Text;

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
    public static string GetDataString(byte[] data, bool printable = false)
    {
      return (printable ? Encoding.ASCII.GetString(data) : Convert.ToBase64String(data));
    }
  }
}
