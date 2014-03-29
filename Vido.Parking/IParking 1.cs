namespace Vido.Parking
{
  public interface IParking
  {
    #region Public Properties
    /// <summary>
    /// Số lượng chỗ đỗ tối đa của Bãi.
    /// </summary>
    int MaximumSlots { get; set; }

    /// <summary>
    /// Trạng thái Bãi đầy.
    /// </summary>
    bool IsFull { get; set; }
    #endregion

    #region Public Methods
    /// <summary>
    /// Kiểm tra xem phương tiện có thể Vào bãi hay không.
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Vào bãi, ngược lại: false</returns>
    bool CanIn(string data, string plateNumber);

    /// <summary>
    /// Xử lý phương tiện Vào bãi.
    /// </summary>
    /// <param name="inArgs">Thông tin phương tiện vào bãi.</param>
    void In(InOutArgs inArgs);

    /// <summary>
    /// Kiểm tra xem phương tiện có thể Ra bãi hay không.
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Ra bãi, ngược lại: false</returns>
    bool CanOut(string data, string plateNumber, ref string inBackImage, ref string inFrontImage);

    /// <summary>
    /// Xử lý phương tiện Ra bãi.
    /// </summary>
    /// <param name="outArgs">Thông tin phương tiện ra.</param>
    void Out(InOutArgs outArgs);
    #endregion
  }
}
