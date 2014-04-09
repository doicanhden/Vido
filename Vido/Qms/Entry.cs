// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;

  /// <summary>
  /// Bảng ghi thông tin vào Làn.
  /// </summary>
  public class Entry
  {
    #region Public Properties
    /// <summary>
    /// Thời gian vào Làn.
    /// </summary>
    public DateTime EntryTime { get; set; }

    /// <summary>
    /// Mã Làn vào.
    /// </summary>
    public string EntryGate { get; set; }

    /// <summary>
    /// Data từ thiết bị Uid.
    /// </summary>
    public string UniqueId { get; set; }

    /// <summary>
    /// Dữ liệu người dùng.
    /// </summary>
    public string UserData { get; set; }

    /// <summary>
    /// Đường dẫn đến ảnh chụp phía Trước.
    /// </summary>
    public string FirstImage { get; set; }

    /// <summary>
    /// Đường dẫn đến ảnh chụp phía Sau.
    /// </summary>
    public string SecondImage { get; set; }
    #endregion

    #region Public Constructors
    public Entry()
    {
    }
    #endregion
  }
}