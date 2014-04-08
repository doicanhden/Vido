﻿namespace Vido.Parking
{
  using System;
  using System.Linq;
  using Vido.Qms;

  public class EFEntryRecorder : IEntryRecorder, IDisposable
  {
    #region Data Members
    private readonly object locker = new object();
    private readonly VidoParkingEntities entities;
    #endregion

    #region Public Events
    /// <summary>
    /// Sự kiện kích hoạt khi nhận được thông báo mới từ Bãi.
    /// </summary>
    public event EventHandler NewMessage;
    #endregion

    #region Public Constructors
    public EFEntryRecorder(int maximumSlots = 15000, int minimumSlots = 0)
    {
      this.entities = new VidoParkingEntities();
      this.MaximumSlots = maximumSlots;
      this.MinimumSlots = minimumSlots;
    }
    #endregion

    public string CurrentUserId { get; set; }
    /// <summary>
    /// Chuỗi định dạng thời gian theo chuẩn ISO-8601
    /// </summary>
    public static string ISO8601DateTimeFormat
    {
      get { return ("yyyy-MM-dd HH:mm:ss.fff"); }
    }

    #region Private Methods
    private void RaiseNewMessage(string message)
    {
      if (NewMessage != null)
      {
        NewMessage(this, new NewMessageEventArgs()
        {
          Message = message
        });
      }
    }
    #endregion

    #region Implementation of IEntryRecorder
    public int MinimumSlots { get; set; }
    public int MaximumSlots { get; set; }

    /// <summary>
    /// Trạng thái Bãi đầy.
    /// </summary>
    public bool IsFull
    {
      get
      {
        lock (locker)
        {
          try
          {
            /// Kiểm tra trạng thái Bãi.
            /// Đếm số phương tiện chưa RA Bãi
            /// và so sánh với số lượng vị trí tối đa.
            return (MaximumSlots <= entities.InOutRecord.Count(r =>
              r.OutEmployeeId == null &&
              r.OutLaneCode == null &&
              r.OutTime == null &&
              r.OutBackImg == null &&
              r.OutFrontImg == null));
          }
          catch
          {
            /// TODO: Địa phương hóa chuỗi thông báo.
            RaiseNewMessage("IParking.IsFull: Lỗi truy xuất dữ liệu.");
            return (true);
          }
        }
      }
    }

    /// <summary>
    /// Kiểm tra xem phương tiện có thể Vào bãi hay không.
    /// </summary>
    /// <param name="uniqueId">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Vào bãi, ngược lại: false</returns>
    public bool CanImport(string uniqueId, string userData)
    {
      /// TODO: Trả về vị trí Phương tiện có thể Đỗ.
      lock (locker)
      {
        try
        {
          var inRecords = from Records in entities.InOutRecord
                          where
                            Records.CardId == uniqueId &&
                            Records.UserData == userData &&
                            Records.OutEmployeeId == null &&
                            Records.OutLaneCode == null &&
                            Records.OutTime == null &&
                            Records.OutBackImg == null &&
                            Records.OutFrontImg == null
                          select Records;

          return (inRecords.Count() == 0);
        }
        catch
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          RaiseNewMessage("IParking.CanIn: Lỗi truy xuất dữ liệu.");
          return (false);
        }
      }
    }

    /// <summary>
    /// Xử lý phương tiện Vào bãi.
    /// </summary>
    /// <param nameLaneEntryRecordRecordRecord>Thông tin phương tiện vào bãi.</param>
    public bool Import(Entry entry)
    {
      lock (locker)
      {
        try
        {
          /// Thêm thông tin phương tiện VÀO.
          entities.InOutRecord.Add(new InOutRecord()
          {
            CardId = entry.UniqueId,
            UserData = entry.UserData,

            InEmployeeId = CurrentUserId,
            InLaneCode = entry.EntryGate,
            InTime = entry.EntryTime.ToString(ISO8601DateTimeFormat),
            InBackImg = entry.BackImage,
            InFrontImg = entry.FrontImage
          });

          /// Cập nhật thông tin vào DB.
          entities.SaveChanges();
          return (true);
        }
        catch
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          RaiseNewMessage("IParking.In: Lỗi truy xuất dữ liệu.");
          return (false);
        }
      }
    }

    /// <summary>
    /// Kiểm tra xem phương tiện có thể Ra bãi hay không.
    /// </summary>
    /// <param name="uniqueId">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Ra bãi, ngược lại: false</returns>
    public bool CanExport(string uniqueId, string userData, out string firstImagePath, out string secondImagePath)
    {
      firstImagePath = null;
      secondImagePath = null;

      lock (locker)
      {
        try
        {
          var inRecords = from Records in entities.InOutRecord
                          where
                            Records.CardId == uniqueId &&
                            Records.UserData == userData &&
                            Records.OutEmployeeId == null &&
                            Records.OutLaneCode == null &&
                            Records.OutTime == null &&
                            Records.OutBackImg == null &&
                            Records.OutFrontImg == null
                          select Records;

          if (inRecords.Count() == 1)
          {
            firstImagePath = inRecords.ToArray()[0].InBackImg;
            secondImagePath = inRecords.ToArray()[0].InFrontImg;

            return (true);
          }

          return (false);
        }
        catch
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          RaiseNewMessage("IParking.CanOut: Lỗi truy xuất dữ liệu.");
          return (false);
        }
      }
    }

    /// <summary>
    /// Xử lý phương tiện Ra bãi.
    /// </summary>
    /// <param name=LaneEntryRecordRecordRecord>Thông tin phương tiện ra.</param>
    public bool Export(Entry entry)
    {
      lock (locker)
      {
        try
        {
          /// Lấy những bản ghi có CardId và PlateNumber khớp,
          /// và chưa có thông tin Ra.
          var inRecords = from Records in entities.InOutRecord
                          where
                            Records.CardId == entry.UniqueId &&
                            Records.UserData == entry.UserData &&
                            Records.OutEmployeeId == null &&
                            Records.OutLaneCode == null &&
                            Records.OutTime == null &&
                            Records.OutBackImg == null &&
                            Records.OutFrontImg == null
                          select Records;

          /// Chỉ có duy nhất 1 bảng ghi khớp,
          /// Nếu có nhiều hơn hoặc không có bản ghi nào 
          /// => Xuất thông báo lỗi Database.
          if (inRecords.Count() == 1)
          {
            var record = inRecords.ToArray()[0];

            /// Thêm thông tin phương tiện RA.
            record.OutEmployeeId = CurrentUserId;
            record.OutLaneCode = entry.EntryGate;
            record.OutTime = entry.EntryTime.ToString(ISO8601DateTimeFormat);
            record.OutBackImg = entry.BackImage;
            record.OutFrontImg = entry.FrontImage;

            /// Cập nhật dữ liệu vào Database.
            entities.SaveChanges();
          }
          else
          {
            /// TODO: Địa phương hóa chuỗi thông báo.
            RaiseNewMessage("IParking.Out: Lỗi CSDL, không có hoặc có nhiều hơn một thông tin phương tiện vào.");
          }

          return (true);
        }
        catch
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          RaiseNewMessage("IParking.Out: Lỗi truy xuất dữ liệu.");
          return (false);
        }
      }
    }

    public void Blocked(Entry entry)
    {
    }
    #endregion

    #region Implementation of IDisposable

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose managed resources
        entities.Dispose();
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}
