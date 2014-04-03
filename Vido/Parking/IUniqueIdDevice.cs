namespace Vido.Parking
{
  using System;

  public interface IUniqueIdDevice
  {
    #region Public Events
    /// <summary>
    /// Sự kiện kích thoạt khi có dữ liệu đến từ thiết bị.
    /// </summary>
    event EventHandler DataIn;
    #endregion

    #region Public Properties
    /// <summary>
    /// Tên của thiết bị sinh dữ liệu Uid.
    /// </summary>
    string Name { get; set; }
    #endregion
  }
}