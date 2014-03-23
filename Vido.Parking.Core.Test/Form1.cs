using System.Windows.Forms;
using Vido.Capture;
using Vido.Parking.Test;
using Vido.Parking.Interfaces;

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

      controller = new Controller(parking, captureFactory, RFIDReaderEnumerator.GetInstance(Handle));
      controller.Lanes.CollectionChanged += Lanes_CollectionChanged;
    }

    private void Lanes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
    }

    private static ISettingsProvider DefaultSetting(string fileName)
    {
      var settings = new Settings(fileName);

      settings.Set(SettingKeys.RootImageDirectoryName, Application.StartupPath + @"\Images");
      settings.Set(SettingKeys.DailyDirectoryFormat, "{0}YYYY{0}MM{0}DD");
      settings.Set(SettingKeys.BackImageNameFormat , "{0}{1}{2}{3}B.jpg");
      settings.Set(SettingKeys.FrontImageNameFormat, "{0}{1}{2}{3}F.jpg");
      settings.Set(SettingKeys.InFormat , "I");
      settings.Set(SettingKeys.OutFormat, "O");

      settings.Set(SettingKeys.Lanes, new LaneConfigs[2]
      {
        new LaneConfigs()
        {
          BackCamera = new CaptureConfigs()
          {
            Source = @"http://64.122.208.241:8000/axis-cgi/mjpg/video.cgi?resolution=320x240"
          },
          FrontCamera = new CaptureConfigs()
          {

          },
          Direction = Enums.Direction.In,
          UidDeviceName = "DDD"
        },
        new LaneConfigs()
        {
          BackCamera = new CaptureConfigs()
          {
            Source = @"http://64.122.208.241:8000/axis-cgi/mjpg/video.cgi?resolution=320x240"
          },
          FrontCamera = new CaptureConfigs()
          {

          },
          Direction = Enums.Direction.Out,
          UidDeviceName = "DDD"
        }
      });

      return (settings);
    }
  }
}
