namespace Vido.Capture
{
  public interface IFactory
  {
    #region Public Methods
    /// <summary>
    /// Tạo thiết bị Chụp ảnh mới dựa vào Cấu hình thiết bị
    /// </summary>
    /// <param name="configs">Cấu hình thiết bị Chụp ảnh</param>
    /// <returns>Đối tượng Chụp ảnh được tạo; nếu không thể tạo thiết bị: null/returns>
    ICapture Create(IConfigs configs);
    #endregion
  }
}