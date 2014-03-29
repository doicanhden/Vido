namespace Vido.Parking
{
  using System;
  using System.Drawing;
  using System.Linq;
  using System.IO;
  using Vido.Parking.Interfaces;
  using Vido.Parking.Utilities;
  using Vido.Parking.Events;

  public class Parking : IParking, IDisposable
  {
    private VidoParkingEntities entities = new VidoParkingEntities();
    private string currentUserId;

    public event NewMessageEventHandler NewMessage;

    public bool UserLogin(string username, string password)
    {

      return (true);
    }
    /// <summary>
    /// Thông tin cài đặt Bãi.
    /// </summary>
    public ISettingsProvider Settings { get; set; }

    /// <summary>
    /// Số lượng chỗ đỗ tối đa của Bãi.
    /// TODO: Đưa vào Settings
    /// </summary>
    public int MaximumSlots { get; set; }

    /// <summary>
    /// Trạng thái Bãi đầy.
    /// </summary>
    public bool IsFull { get; private set; }

    /// <summary>
    /// Kiểm tra xem phương tiện có thể Ra bãi hay không.
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <param name="plateNumber">Biển số phương tiện</param>
    /// <returns>true - Nếu phương tiện có thể Ra bãi, ngược lại: false</returns>
    public bool CanOut(byte[] data, string plateNumber)
    {
      // TODO: Kiểm tra lại Logic.
      // TODO: Chỉ nên kiểm Tra thời gian?
      var encodeData = Encode.EncodeData(data);
      var inRecords = from Records in entities.InOutRecord
                      where
                        Records.UserData == plateNumber &&
                        Records.CardId == encodeData &&
                        // Phương tiện chưa ra khỏi bãi.
                        Records.OutUserId == null &&
                        Records.OutLaneCode == null &&
                        Records.OutTime == null &&
                        Records.OutBackImg == null &&
                        Records.OutFrontImg == null
                      select Records;

      return (inRecords.Count() == 1);
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
      // TODO: Thêm Kiểm tra trạng thái thẻ (Bảng Card).
      // TODO: Kiểm tra lại Logic.
      // TODO: Chỉ nên kiểm Tra thời gian?

      if (IsFull)
      {
        RaiseNewMessage("Bãi đã đầy.");
        return (false);
      }

      var encodeData = Encode.EncodeData(data);

      var inRecords = from Records in entities.InOutRecord
                      where
                        Records.CardId == encodeData &&
                        Records.OutUserId == null &&
                        Records.OutLaneCode == null &&
                        Records.OutTime == null &&
                        Records.OutBackImg == null &&
                        Records.OutFrontImg == null
                      select Records;

      return (inRecords.Count() == 0);
    }

    /// <summary>
    /// Xử lý phương tiện Ra bãi.
    /// </summary>
    /// <param name="outArgs">Thông tin phương tiện ra.</param>
    public void Out(InOutArgs outArgs)
    {
      // TODO: Kiểm tra lại Logic.
      // TODO: Chỉ nên kiểm Tra thời gian?

      var encodeData = Encode.EncodeData(outArgs.Data);

      /* Lấy những bản ghi có CardId và PlateNumber khớp,
       * và chưa có thông tin Ra.
       */
      var inRecords = from Records in entities.InOutRecord
                      where
                        Records.CardId == encodeData &&
                        Records.UserData == outArgs.PlateNumber &&
                        Records.OutUserId == null &&
                        Records.OutLaneCode == null &&
                        Records.OutTime == null &&
                        Records.OutBackImg == null &&
                        Records.OutFrontImg == null
                      select Records;

      /* Chỉ có duy nhất 1 bảng ghi khớp,
       * Nếu có nhiều hơn hoặc không có bản ghi nào 
       * => Xuất thông báo lỗi Database.
       */
      if (inRecords.Count() == 1)
      {
        inRecords.ToList().ForEach((r) =>
        {
          r.OutUserId = currentUserId;
          r.OutLaneCode = outArgs.LaneCode;
          r.OutTime = outArgs.Time.ToString(ISO8601DateTimeFormat);
          r.OutBackImg = outArgs.BackImage;
          r.OutFrontImg = outArgs.FrontImage;
        });
        entities.SaveChanges();
        // Đặt lại trạng thái Bãi chưa đầy.
        IsFull = false;
      }
      else
      {
        RaiseNewMessage("Lỗi CSDL, không có hoặc có nhiều hơn một thông tin phương tiện vào.");
      }
    }

    /// <summary>
    /// Xử lý phương tiện Vào bãi.
    /// </summary>
    /// <param name="inArgs">Thông tin phương tiện vào bãi.</param>
    public void In(InOutArgs inArgs)
    {
      entities.InOutRecord.Add(new InOutRecord()
      {
        CardId = Encode.EncodeData(inArgs.Data),
        UserData = inArgs.PlateNumber,

        InUserId = currentUserId,
        InLaneCode = inArgs.LaneCode,
        InTime = inArgs.Time.ToString(ISO8601DateTimeFormat),
        InBackImg = inArgs.BackImage,
        InFrontImg = inArgs.FrontImage
      });
      entities.SaveChanges();
      // TODO: Kiểm tra bãi đầy hay chưa.
      // Đếm số phương tiện chưa Ra bãi và so sánh với số lượng vị trí tối đa.
      IsFull = entities.InOutRecord.Count(IORecordIsNullOut) >= MaximumSlots;
    }

    /// <summary>
    /// Kiểm tra bản ghi I/O có phần Out là Null.
    /// </summary>
    /// <param name="record">Bản ghi cần kiểm tra</param>
    /// <returns>true - nếu chưa có thông tin OUT, ngược lại: false</returns>
    private bool IORecordIsNullOut(InOutRecord record)
    {
      return (
        record.OutUserId == null &&
        record.OutLaneCode == null &&
        record.OutTime == null &&
        record.OutBackImg == null &&
        record.OutFrontImg == null);
    }

    private void RaiseNewMessage(string message)
    {
      if (NewMessage != null)
      {
        NewMessage(this, new NewMessageEventArgs(message));
      }
    }

    /// <summary>
    /// Chuỗi định dạng thời gian theo chuẩn ISO-8601
    /// </summary>
    private static string ISO8601DateTimeFormat
    {
      get { return ("yyyy-MM-dd HH:mm:ss.fff"); }
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
