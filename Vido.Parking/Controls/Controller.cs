namespace Vido.Parking.Controls
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.IO;
  using Vido.Capture;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Utilities;

  /// <summary>
  /// Controller: Điều khiển các Lane
  /// </summary>
  public class Controller
  {
    #region Data Members
    private readonly IParking parking = null;
    private readonly ICardManagement card = null;
    private readonly IFactory captureFactory = null;
    private readonly IUidDeviceList inputDevices = null;
    private readonly ICollection<Lane> lanes = new List<Lane>();
    #endregion

    #region Public Properties
    public ICollection<Lane> Lanes
    {
      get { return (lanes); }
    }

    public ICollection<LaneConfigs> LaneConfigs { get; set; }

    /// <summary>
    /// Chuỗi quy ước tên ảnh chụp phương tiện ra.
    /// </summary>
    public string InFormat { get; set; }

    /// <summary>
    /// Chuỗi quy ước tên ảnh chụp phương tiện ra.
    /// </summary>
    public string OutFormat { get; set; }

    /// <summary>
    /// Chuỗi định dạng tên tệp tin ảnh chụp Biển số phương tiện.
    /// {0} - Time,
    /// {1} - Uid data,
    /// {2} - In/Out,
    /// {3} - Lane code,
    /// {4} - Plate number
    /// </summary>
    public string BackImageNameFormat { get; set; }

    /// <summary>
    /// Chuỗi định dạng tên tệp tin ảnh chụp Người điều khiển phương tiện.
    /// {0} - Time,
    /// {1} - Uid data,
    /// {2} - In/Out,
    /// {3} - Lane code,
    /// {4} - Plate number
    /// </summary>
    public string FrontImageNameFormat { get; set; }

    /// <summary>
    /// Chuỗi dạng đường dẫn thư mục hằng ngày,
    /// {0} - Directory separator char,
    /// yyyy - year,
    /// MM - month,
    /// dd - day
    /// </summary>
    public string DailyDirectoryFormat { get; set; }

    /// <summary>
    /// Đường dẫn thư mục gốc chứa ảnh.
    /// </summary>
    public string RootImageDirectoryName { get; set; }
    #endregion

    #region Public Constructors
    public Controller(IParking parking, ICardManagement card, IUidDeviceList inputDevices, IFactory captureFactory)
    {
      if (parking == null)
      {
        throw new ArgumentNullException("parking");
      }

      if (card == null)
      {
        throw new ArgumentNullException("card");
      }

      if (inputDevices == null)
      {
        throw new ArgumentNullException("inputDevices");
      }

      if (captureFactory == null)
      {
        throw new ArgumentNullException("captureFactory");
      }

      this.parking = parking;
      this.card = card;
      this.captureFactory = captureFactory;
      this.inputDevices = inputDevices;

      GenerateLanes();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Tạo ra các Lane từ config của Parking
    /// </summary>
    public void GenerateLanes()
    {
      if (lanes.Count > 0)
      {
        foreach (var lane in lanes)
        {
          // Hủy đăng kí sự kiện với Lane
          lane.Entry -= lane_Entry;
          inputDevices.Unregister(lane.UidDevice);
        }

        lanes.Clear();
      }

      if (LaneConfigs != null)
      {
        foreach (var cfg in LaneConfigs)
        {
          var lane = new Lane()
          {
            Code = cfg.Code,
            Direction = cfg.Direction,
            NumberOfRetries = cfg.NumberOfRetries,
            LaneState = cfg.State
          };

          // Tạo các Camera cho Lane.
          lane.FrontCamera = captureFactory.Create(cfg.FrontCamera);
          lane.BackCamera = captureFactory.Create(cfg.BackCamera);

          // Đăng kí input từ thiết bị.
          lane.UidDevice = inputDevices.Register(cfg.UidDeviceName);

          // Đăng kí sự kiện với Lane
          lane.Entry += lane_Entry;

          lanes.Add(lane);
        }
      }
    }

    #endregion

    #region Event Handlers
    private void lane_Entry(object sender, EventArgs e)
    {
      var lane = sender as Lane;
      var args = e as EntryEventArgs;

      if (lane != null && args != null)
      {
        var data = Encode.GetDataString(args.DataIn.Data, args.DataIn.Printable);

        if (!card.IsExistAndUsing(data))
        {
          /// TODO: Địa phương hóa chuỗi thông báo.
          args.Message = "Thẻ không hợp lệ.";
          return;
        }

        var inOut = new InOutArgs()
        {
          Time = args.Time,
          Data = data,
          Lane = lane.Code,
          PlateNumber = args.PlateNumber
        };

        switch (lane.Direction)
        {
          case Direction.In:
            if (parking.IsFull)
            {
              /// TODO: Địa phương hóa chuỗi thông báo.
              args.Message = "Bãi đã đầy.";
            }
            else
            {
              if (!parking.CanIn(data, args.PlateNumber))
              {
                /// TODO: Địa phương hóa chuỗi thông báo.
                args.Message = "Xe KHÔNG ĐƯỢC PHÉP VÀO bãi.";
                return;
              }

              if (!SaveImages(args.BackImage, args.FrontImage, InFormat, ref inOut))
              {
                /// TODO: Địa phương hóa chuỗi thông báo.
                args.Message = "Không lưu được ảnh.";
              }

              parking.In(inOut);

              /// TODO: Địa phương hóa chuỗi thông báo.
              args.Message = "Mời xe VÀO Bãi.";
              args.Allow = true;
            }
            break;
          case Direction.Out:
            {
              string inBackImage = string.Empty;
              string inFrontImage = string.Empty;

              if (!parking.CanOut(data, args.PlateNumber, ref inBackImage, ref inFrontImage))
              {
                /// TODO: Địa phương hóa chuỗi thông báo.
                args.Message = "Xe KHÔNG ĐƯỢC PHÉP RA Bãi.";
                return;
              }

              if (!SaveImages(args.BackImage, args.FrontImage, OutFormat, ref inOut))
              {
                /// TODO: Địa phương hóa chuỗi thông báo.
                args.Message = "Không lưu được ảnh.";
                return;
              }

              parking.Out(inOut);

              if (File.Exists(RootImageDirectoryName + inBackImage))
              {
                args.BackImage = Bitmap.FromFile(RootImageDirectoryName + inBackImage);
              }
              else
              {
                /// TODO: Địa phương hóa chuỗi thông báo.
                args.Message = "Không tìm thấy ảnh chụp biển số.";
                args.BackImage = null;
                return;
              }

              if (File.Exists(RootImageDirectoryName + inFrontImage))
              {
                args.FrontImage = Bitmap.FromFile(RootImageDirectoryName + inFrontImage);
              }
              else
              {
                /// TODO: Xử lý trường hợp Lane vào không chụp ảnh Người điều khiển.
                /// Kiểm tra config của Lane đó trong settings dựa vào LaneCode.
                args.FrontImage = null;
              }

              args.Message = "Mời xe RA Bãi";
              args.Allow = true;
            }
            break;
          default:
            break;
        }
      }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Lưu ảnh Biển số và Người điều khiển xuống thư mục RootImage.
    /// </summary>
    /// <param name="back">Ảnh chụp Biển số</param>
    /// <param name="front">Ảnh chụp người điều khiển</param>
    /// <param name="inOutFormat">Chuỗi định dạng In/Out</param>
    /// <param name="inOutArgs">Thông tin Vào/Ra bãi</param>
    /// <returns>true - Nếu lưu ảnh thành công, ngược lại: false</returns>
    private bool SaveImages(Image back, Image front, string inOutFormat, ref InOutArgs inOutArgs)
    {
      try
      {
        var dailyDirectory = inOutArgs.Time.ToString(Path.DirectorySeparatorChar == '\\' ?
          string.Format(DailyDirectoryFormat, @"\\") :
          string.Format(DailyDirectoryFormat, Path.DirectorySeparatorChar));

        var timeString = inOutArgs.Time.ToString("HHmmss");

        CreateDirectoryIfNotExists(RootImageDirectoryName + dailyDirectory);
        dailyDirectory += Path.DirectorySeparatorChar;

        if (back != null)
        {
          inOutArgs.BackImage = dailyDirectory + string.Format(BackImageNameFormat,
            timeString, inOutArgs.Data, inOutFormat, inOutArgs.Lane, inOutArgs.PlateNumber);

          back.Save(RootImageDirectoryName + inOutArgs.BackImage);
        }

        if (front != null)
        {
          inOutArgs.FrontImage = dailyDirectory + string.Format(FrontImageNameFormat,
            timeString, inOutArgs.Data, inOutFormat, inOutArgs.Lane, inOutArgs.PlateNumber);

          front.Save(RootImageDirectoryName + inOutArgs.FrontImage);
        }

        return (true);
      }
      catch
      {
        /// TODO: Thêm xử lý exception ở đây.
        return (false);
      }
    }

    /// <summary>
    /// Tạo thư mục nếu thư mục không tồn tại
    /// </summary>
    /// <param name="directoryName">Đường dẫn thư mục cần tạo</param>
    private static void CreateDirectoryIfNotExists(string directoryName)
    {
      if (!Directory.Exists(directoryName))
      {
        Directory.CreateDirectory(directoryName);
      }
    }
    #endregion
  }
}