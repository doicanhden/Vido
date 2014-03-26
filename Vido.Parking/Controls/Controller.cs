namespace Vido.Parking.Controls
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.IO;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Controls;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;

  /// <summary>
  /// Controller: Điều khiển các Lane
  /// </summary>
  public class Controller
  {
    #region Data Members
    private readonly IParking parking = null;
    private readonly ICaptureFactory captureFactory = null;
    private readonly IUidDeviceList inputDevices = null;
    private readonly List<Lane> lanes = new List<Lane>();
    #endregion

    #region Public Properties
    public ICollection<Lane> Lanes
    {
      get { return (lanes); }
    }
    #endregion

    #region Public Constructors
    public Controller(IParking parking, IUidDeviceList inputDevices, ICaptureFactory captureFactory)
    {
      if (parking == null)
      {
        throw new ArgumentNullException("parking");
      }

      if (captureFactory == null)
      {
        throw new ArgumentNullException("captureFactory");
      }

      if (inputDevices == null)
      {
        throw new ArgumentNullException("inputDevices");
      }

      this.parking = parking;
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
            Direction = cfg.Direction,
            NumberOfRetries = cfg.NumberOfRetries,
            State = cfg.State
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
    private void lane_Entry(object sender, EntryEventArgs e)
    {
      var lane = sender as Lane;
      if (lane != null)
      {
        var inOut = new InOutArgs()
        {
          Time = DateTime.Now,
          Data = e.Data,
          PlateNumber = e.PlateNumber
        };

        switch (lane.Direction)
        {
          case Direction.In:
            if (parking.CanIn(e.Data, e.PlateNumber) &&
              SaveImages(e.BackImage, e.FrontImage, InFormat, ref inOut))
            {
              parking.In(inOut);
            }
            else
            {
              e.Allow = false;
            }
            break;
          case Direction.Out:
            if (parking.CanOut(e.Data, e.PlateNumber) &&
              SaveImages(e.BackImage, e.FrontImage, OutFormat, ref inOut))
            {
              parking.Out(inOut);
            }
            else
            {
              e.Allow = false;
            }
            break;
          default:
            break;
        }
      }
    }
    #endregion

    /// <summary>
    /// Mã hóa dữ liệu Uid thành dạng có thể In
    /// </summary>
    /// <param name="data">Dữ liệu Uid</param>
    /// <returns>Chuỗi có thể In</returns>
    public static string EncodeData(byte[] data)
    {
      return (Convert.ToBase64String(data));
    }

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
        var dataBase64 = EncodeData(inOutArgs.Data);

        CreateDirectoryIfNotExists(RootImageDirectoryName + dailyDirectory);
        dailyDirectory += Path.DirectorySeparatorChar;

        if (back != null)
        {
          inOutArgs.BackImage = dailyDirectory + string.Format(BackImageNameFormat,
            timeString, inOutFormat, inOutArgs.PlateNumber, dataBase64);

          back.Save(RootImageDirectoryName + inOutArgs.BackImage);
        }

        if (front != null)
        {
          inOutArgs.FrontImage = dailyDirectory + string.Format(FrontImageNameFormat,
            timeString, inOutFormat, inOutArgs.PlateNumber, dataBase64);

          front.Save(RootImageDirectoryName + inOutArgs.FrontImage);
        }

        return (true);
      }
      catch
      {
        throw;
      }
    }

    /// <summary>
    /// Tạo thư mục nếu thư mục không tồn tại
    /// </summary>
    /// <param name="directoryName">Đường dẫn thư mục cần tạo</param>
    private void CreateDirectoryIfNotExists(string directoryName)
    {
      if (!Directory.Exists(directoryName))
      {
        Directory.CreateDirectory(directoryName);
      }
    }
    #endregion

    #region Setting Accessors
    private LaneConfigs[] LaneConfigs
    {
      get { return (parking.Settings.Query<LaneConfigs[]>(SettingKeys.Lanes)); }
    }
    private string InFormat
    {
      get { return (parking.Settings.Query<string>(SettingKeys.InFormat)); }
    }
    private string OutFormat
    {
      get { return (parking.Settings.Query<string>(SettingKeys.OutFormat)); }
    }
    private string RootImageDirectoryName
    {
      get { return (parking.Settings.Query<string>(SettingKeys.RootImageDirectoryName)); }
    }
    private string DailyDirectoryFormat
    {
      get { return (parking.Settings.Query<string>(SettingKeys.DailyDirectoryFormat)); }
    }
    public string FrontImageNameFormat
    {
      get { return (parking.Settings.Query<string>(SettingKeys.FrontImageNameFormat)); }
    }
    public string BackImageNameFormat
    {
      get { return (parking.Settings.Query<string>(SettingKeys.BackImageNameFormat)); }
    }
    #endregion
  }
}
