
namespace Vido.Parking.Ui.Wpf.ViewModels
{
  using System;
  using System.Windows;
  using System.Windows.Interop;
  using System.Collections.ObjectModel;
  using Vido.Capture;
  using Vido.Capture.Enums;
  using Vido.Parking.Interfaces;
  using Vido.Parking.Controls;
  using System.Drawing;
  using System.Diagnostics;
  public class MainViewModel : IDisposable
  {
    private ISettingsProvider settings = null;
    private readonly InputDeviceList inputDevices;
    private readonly CaptureList captures;
    private readonly Parking parking;
    private readonly Controller controller;
    private readonly ObservableCollection<LaneViewModel> laneViewModels;

    public MainViewModel(IntPtr mainWindowsHandle)
    {
      settings = TestSetting(@"\settings.xml");
      settings.Save();

      inputDevices = InputDeviceList.GetInstance(mainWindowsHandle);
      captures = new CaptureList(new CaptureFactory());
      laneViewModels = new ObservableCollection<LaneViewModel>();

      parking = new Parking();
      parking.Settings = settings;
      parking.MaximumSlots = 1000;

      controller = new Controller(parking, inputDevices, captures)
      {
        FrontImageNameFormat = settings.Query<string>(SettingKeys.FrontImageNameFormat),
        BackImageNameFormat = settings.Query<string>(SettingKeys.BackImageNameFormat),

        DailyDirectoryFormat = settings.Query<string>(SettingKeys.DailyDirectoryFormat),
        InFormat = settings.Query<string>(SettingKeys.InFormat),
        OutFormat = settings.Query<string>(SettingKeys.OutFormat),
        LaneConfigs = settings.Query<LaneConfigs[]>(SettingKeys.Lanes),
        RootImageDirectoryName = settings.Query<string>(SettingKeys.RootImageDirectoryName),

        EncodeData = false
      };

      controller.GenerateLanes();

      parking.Settings.SettingChanged += (s, e) =>
      {
        if (e.SettingKey == SettingKeys.Lanes)
        {
          controller.GenerateLanes();
        }
      };
      GenerateLaneViewModels();

      foreach (var cap in captures.Captures)
      {
       cap.Start();
      }
    }


    /// <summary>
    /// Tạo ViewModel cho từng Lane.
    /// </summary>
    private void GenerateLaneViewModels()
    {
      laneViewModels.Clear();

      foreach (var lane in controller.Lanes)
      {
        laneViewModels.Add(new LaneViewModel(lane));
      }
    }

    /// <summary>
    /// Danh sách các ViewModel của lane.
    /// </summary>
    public ObservableCollection<LaneViewModel> Lanes
    {
      get { return (laneViewModels); }
    }
    private static ISettingsProvider TestSetting(string fileName)
    {
      var settings = new Settings(fileName);

      settings.Set(SettingKeys.RootImageDirectoryName, Environment.CurrentDirectory + @"\Images");
      settings.Set(SettingKeys.DailyDirectoryFormat, "{0}yyyy{0}MM{0}dd");
      settings.Set(SettingKeys.BackImageNameFormat , "IMG_{0}_{1}_{2}_{3}_B.jpg");
      settings.Set(SettingKeys.FrontImageNameFormat, "IMG_{0}_{1}_{2}_{3}_F.jpg");
      settings.Set(SettingKeys.InFormat, "I");
      settings.Set(SettingKeys.OutFormat, "O");

      settings.Set(SettingKeys.Lanes, new LaneConfigs[1]
      {
        new LaneConfigs()
        {
          BackCamera = new CaptureConfigs()
          {
            Source = @"http://camera1.mairie-brest.fr/mjpg/video.mjpg?resolution=320x240",
            Coding = Coding.MJpeg,
            Username = "admin",
            Password = "admin"
          },
          FrontCamera = new CaptureConfigs()
          {
            Source = @"http://camera1.mairie-brest.fr/mjpg/video.mjpg?resolution=320x240",
            Coding = Coding.MJpeg,
            Username = "admin",
            Password = "admin"
          },
          Direction = Enums.Direction.In,
          UidDeviceName = @"VID_0E6A&PID_030B",
          NumberOfRetries = 3,
          State = Enums.LaneState.Ready
        }
      });

      return (settings);
    }

    private static IntPtr MainWindowsHandle
    {
      get { return (new WindowInteropHelper(Application.Current.MainWindow).Handle); }
    }

    #region Implementation of IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose managed resources
        captures.Dispose();
        parking.Dispose();
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
