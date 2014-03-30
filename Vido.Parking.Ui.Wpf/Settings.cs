namespace Vido.Parking.Ui.Wpf
{
  using System;
using System.Collections.Generic;
using System.IO;
using Vido.Capture;
using Vido.Parking.Controls;

  public class Settings
  {
    private readonly Datasets.Settings.ParkingConfigsDataTable parkingCfgs = null;
    private readonly Datasets.Settings.ControllerConfigsDataTable controllerCfgs = null;
    private readonly Datasets.Settings.LaneConfigsDataTable laneCfgs = null;

    public static string SettingsDirectoryName
    {
      get { return (Environment.CurrentDirectory + @"\Settings"); }
    }
    public static string ParkingFileName
    {
      get { return (SettingsDirectoryName + @"\Parkings.xml"); }
    }
    public static string ControllerFileName
    {
      get { return (SettingsDirectoryName + @"\Controllers.xml"); }
    }
    public static string LaneFileName
    {
      get { return (SettingsDirectoryName + @"\Lanes.xml"); }
    }

    public Settings()
    {
      parkingCfgs = new Datasets.Settings.ParkingConfigsDataTable();
      controllerCfgs = new Datasets.Settings.ControllerConfigsDataTable();
      laneCfgs = new Datasets.Settings.LaneConfigsDataTable();

      if (!Directory.Exists(SettingsDirectoryName))
      {
        Directory.CreateDirectory(SettingsDirectoryName);
      }

      Load();
    }

    public void Save()
    {
      parkingCfgs.WriteXml(ParkingFileName);
      controllerCfgs.WriteXml(ControllerFileName);
      laneCfgs.WriteXml(LaneFileName);
    }
    public void Load()
    {
      if (!File.Exists(ParkingFileName))
      {
        DefaultParkingConfigs();
        parkingCfgs.WriteXml(ParkingFileName);
      }
      else
      {
        parkingCfgs.ReadXml(ParkingFileName);
      }

      if (!File.Exists(ControllerFileName))
      {
        DefaultControllerConfigs();
        controllerCfgs.WriteXml(ControllerFileName);
      }
      else
      {
        controllerCfgs.ReadXml(ControllerFileName);
      }

      if (!File.Exists(LaneFileName))
      {
        DefaultLaneConfigs();
        laneCfgs.WriteXml(LaneFileName);
      }
      else
      {
        laneCfgs.ReadXml(LaneFileName);
      }
    }

    public void SetParking(IParking parking, int level = 0)
    {
      foreach (Datasets.Settings.ParkingConfigsRow cfg in parkingCfgs.Rows)
      {
        if (cfg.Level == level)
        {
          parking.MaximumSlots = cfg.MaximumSlots;
          break;
        }
      }
    }
    
    public void SetController(Controller controller, int id = 0)
    {
      foreach (Datasets.Settings.ControllerConfigsRow cfg in controllerCfgs.Rows)
      {
        if (cfg.Id == id)
        {
          controller.RootImageDirectoryName = cfg.RootImageDirectoryName;
          controller.DailyDirectoryFormat = cfg.DailyDirectoryFormat;

          controller.BackImageNameFormat = cfg.BackImageNameFormat;
          controller.FrontImageNameFormat = cfg.FrontImageNameFormat;

          controller.InFormat = cfg.InFormat;
          controller.OutFormat = cfg.OutFormat;

          controller.LaneConfigs = new List<LaneConfigs>();
          break;
        }
      }
    }
    public void SetLaneConfigs(ICollection<LaneConfigs> lanes)
    {
      foreach (Datasets.Settings.LaneConfigsRow cfg in laneCfgs.Rows)
      {
        lanes.Add(new LaneConfigs()
        {
          Code = cfg.Code,
          Direction = (Enums.Direction)cfg.Direction,
          State = (Enums.LaneState)cfg.State,
          NumberOfRetries = cfg.NumberOfRetries,
          UidDeviceName = cfg.UIdDeviceName,

          BackCamera = new Configs()
          {
            Source = cfg.BackCamSource,
            Coding = (Capture.Enums.Coding)cfg.BackCamCoding,
            Username = cfg.BackCamUsername,
            Password = cfg.BackCamPassword,
            FrameInterval = 100
          },
          FrontCamera = new Configs()
          {
            Source = cfg.FrontCamSource,
            Coding = (Capture.Enums.Coding)cfg.FrontCamCoding,
            Username = cfg.FrontCamUsername,
            Password = cfg.FrontCamPassword,
            FrameInterval = 100
          }
        });
      }
    }

    private void DefaultLaneConfigs()
    {
      var newRow1 = laneCfgs.NewRow() as Datasets.Settings.LaneConfigsRow;

      newRow1.Code = "LANE1";
      newRow1.State = (int)(Enums.LaneState.Ready);
      newRow1.Direction = (int)(Enums.Direction.In);
      newRow1.UIdDeviceName = @"VID_0E6A&PID_030B";
      newRow1.NumberOfRetries = 3;

      newRow1.BackCamSource = "http://camera1.mairie-brest.fr/mjpg/video.mjpg?resolution=320x240";
      newRow1.BackCamCoding = (int)(Capture.Enums.Coding.MJpeg);
      newRow1.BackCamUsername = "admin";
      newRow1.BackCamPassword = "admin";

      newRow1.FrontCamSource = "http://camera1.mairie-brest.fr/mjpg/video.mjpg?resolution=320x240";
      newRow1.FrontCamCoding = (int)(Capture.Enums.Coding.MJpeg);
      newRow1.FrontCamUsername = "admin";
      newRow1.FrontCamPassword = "admin";

      laneCfgs.Rows.Add(newRow1);


      var newRow2 = laneCfgs.NewRow() as Datasets.Settings.LaneConfigsRow;

      newRow2.Code = "LANE2";
      newRow2.State = (int)(Enums.LaneState.Ready);
      newRow2.Direction = (int)(Enums.Direction.Out);
      newRow2.UIdDeviceName = @"PNP0303";
      newRow2.NumberOfRetries = 3;

      newRow2.BackCamSource = "http://camera1.mairie-brest.fr/mjpg/video.mjpg?resolution=320x240";
      newRow2.BackCamCoding = (int)(Capture.Enums.Coding.MJpeg);
      newRow2.BackCamUsername = "admin";
      newRow2.BackCamPassword = "admin";

      newRow2.FrontCamSource = "http://camera1.mairie-brest.fr/mjpg/video.mjpg?resolution=320x240";
      newRow2.FrontCamCoding = (int)(Capture.Enums.Coding.MJpeg);
      newRow2.FrontCamUsername = "admin";
      newRow2.FrontCamPassword = "admin";

      laneCfgs.Rows.Add(newRow2);
    }

    private void DefaultControllerConfigs()
    {
      var newRow = controllerCfgs.NewRow() as Datasets.Settings.ControllerConfigsRow;

      newRow.Id = 0;
      newRow.RootImageDirectoryName = Environment.CurrentDirectory + @"\Images";
      newRow.DailyDirectoryFormat = "{0}yyyy{0}MM{0}dd";
      newRow.BackImageNameFormat = "IMG_{0}_{1}_{2}{3}_B{4}.jpg";
      newRow.FrontImageNameFormat = "IMG_{0}_{1}_{2}{3}_F{4}.jpg";
      newRow.InFormat = "I";
      newRow.OutFormat = "O";

      controllerCfgs.Rows.Add(newRow);
    }

    private void DefaultParkingConfigs()
    {
      var newRow = parkingCfgs.NewRow() as Datasets.Settings.ParkingConfigsRow;

      newRow.Level = 0;
      newRow.MaximumSlots = 100000;

      parkingCfgs.Rows.Add(newRow);
    }
  }
}
