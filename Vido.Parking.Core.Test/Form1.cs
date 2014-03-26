using System.Windows.Forms;
using Vido.Capture;
using Vido.Parking.Test;
using Vido.Parking.Interfaces;
using Vido.Capture.Enums;
using Vido.Parking.Core.Test;
using Vido.Parking.Controls;
using System;

namespace Vido.Parking.Test
{

  public partial class Form1 : Form
  {
    private CaptureFactory captureFactory = new CaptureFactory();
    private Controller controller = null;
    private Parking parking = null;
    public Form1()
    {
      InitializeComponent();

      parking = new Parking();
      parking.Settings = DefaultSetting(Application.StartupPath + @"\settings.xml");
      parking.Settings.Save();

      controller = new Controller(parking, InputDeviceList.GetInstance(Handle), captureFactory);
    }

    void FrontCamera_NewFrame(object sender, Capture.Events.NewFrameEventArgs e)
    {
      pictureBox2.Image = e.Bitmap;
    }

    void BackCamera_NewFrame(object sender, Capture.Events.NewFrameEventArgs e)
    {
      pictureBox1.Image = e.Bitmap;
    }

    private void Lanes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
    }

    private static ISettingsProvider DefaultSetting(string fileName)
    {
      var settings = new Settings(fileName);

      settings.Set(SettingKeys.RootImageDirectoryName, Application.StartupPath + @"\Images");
      settings.Set(SettingKeys.DailyDirectoryFormat, "{0}yyyy{0}MM{0}dd");
      settings.Set(SettingKeys.BackImageNameFormat , "{0}{1}{2}{3}B.jpg");
      settings.Set(SettingKeys.FrontImageNameFormat, "{0}{1}{2}{3}F.jpg");
      settings.Set(SettingKeys.InFormat , "I");
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
  }
}
