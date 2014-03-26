namespace Vido.Parking.Interfaces
{
  using System;

  /// <summary>
  /// Tham số truyền cho IParking xử lý Vào/Ra.
  /// </summary>
  public class InOutArgs
  {
    #region Public Constructors
    public InOutArgs()
    {
    }
    public InOutArgs(DateTime time, byte[] data, string plateNumber, string backImage, string frontImage)
    {
      this.Time = time;
      this.Data = data;
      this.PlateNumber = plateNumber;
      this.BackImage = backImage;
      this.FrontImage = frontImage;
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Thời gian phương tiện Vào/Ra bãi.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Data từ thiết bị Uid.
    /// </summary>
    public byte[] Data { get; set; }

    /// <summary>
    /// Mã Làn phương tiện Vào/Ra.
    /// </summary>
    public string LaneCode { get; set; }

    /// <summary>
    /// Biển số phương tiện vào.
    /// </summary>
    public string PlateNumber { get; set; }

    /// <summary>
    /// Đường dẫn đến ảnh chụp Biển số.
    /// Không bao gồm RootImageDirectoryName.
    /// </summary>
    public string BackImage { get; set; }

    /// <summary>
    /// Đường dẫn đến ảnh chụp Người điều khiển.
    /// Không bao gồm RootImageDirectoryName.
    /// </summary>
    public string FrontImage { get; set; }
    #endregion
  }
}
