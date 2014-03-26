
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
  public class MainViewModel : IDisposable
  {
    private readonly InputDeviceList inputDevices = InputDeviceList.GetInstance(MainWindowsHandle);
    private readonly CaptureList captures;
    private readonly Parking parking;
    private readonly Controller controller;
    private readonly ObservableCollection<LaneViewModel> laneViewModels;

    public MainViewModel()
    {
      captures = new CaptureList(new CaptureFactory());
      laneViewModels = new ObservableCollection<LaneViewModel>();

      parking = new Parking();
      parking.Settings = DefaultSetting(@"\settings.xml");
      parking.Settings.Save();

      controller = new Controller(parking, inputDevices, captures);

      parking.Settings.SettingChanged += (s, e) =>
      {
        if (e.SettingKey == SettingKeys.Lanes)
        {
          controller.GenerateLanes();
        }
      };

      GenerateLaneViewModels();
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


    private static ISettingsProvider DefaultSetting(string fileName)
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
            Source = @"http://64.122.208.241:8000/axis-cgi/mjpg/video.cgi?resolution=320x240",
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
          UidDeviceName = @"\\?\HID#VID_0E6A&PID_030B#6&bb84ee5&1&0000#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}",
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
