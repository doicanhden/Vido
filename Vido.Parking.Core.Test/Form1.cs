
namespace Vido.Parking.Core.Test
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using System.Xml.Serialization;
  using Vido.Capture;
  using Vido.Capture.Events;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Core.Interfaces;
  using Vido.RawInput.Events;
  using Vido.RawInput.Interfaces;

  public partial class Form1 : Form
  {
    SQLiteDatabase db = new SQLiteDatabase(@"E:\vidoparking.s3db");
    RFIDReader rfidReader = new RFIDReader();
    RawInput.RawInput rawInput = null;
    ILane lane = null;
    public Form1()
    {
      InitializeComponent();

      lane = new Lane()
      {
        Name = "lane 1",
        PlateCamera = new JpegStream()
        {
          Configs = new CaptureConfigs()
          {
            Source = @"http://pasteldth.dyndns.org/cgi-bin/net_jpeg.cgi?ch=2",
            Username = "admin",
            Password = "123456",
            FrameInterval = 0
          }
        },
        FaceCamera = new MJpegStream()
        {
          Configs = new CaptureConfigs()
          {
            Source = @"http://64.122.208.241:8000/axis-cgi/mjpg/video.cgi?resolution=320x240",
            Username = "admin",
            Password = "admin",
            FrameInterval = 0
          }
        }
      };

      lane.PlateCamera.NewFrame += capture1_NewFrame;
      lane.FaceCamera.NewFrame += capture2_NewFrame;

      lane.FaceCamera.Start();
      lane.PlateCamera.Start();
      lane.UidDevice = rfidReader;
      rfidReader.Uid += rfidReader_UidRead;
      rawInput = new RawInput.RawInput(Handle);
      rawInput.AddMessageFilter();

      //    rawInput.Keyboard.DevicesChanged += Keyboard_DevicesChanged;
      //    rawInput.Keyboard.EnumerateDevices();
      
      var s = SerializeToString(lane);
      MessageBox.Show(s);
    }

    void Keyboard_DevicesChanged(IRawKeyboard s, DevicesChangedEventArgs e)
    {
      rfidReader.Keyboard = e.NewDevices[3];
    }

    void rfidReader_UidRead(object s, UidEventArgs e)
    {
      Utilites.Entry(db, e.Uid, lane);
    }

    void capture2_NewFrame(ICapture sender, NewFrameEventArgs e)
    {
      pictureBox2.Image = e.Bitmap;
    }

    void capture1_NewFrame(ICapture sender, NewFrameEventArgs e)
    {
      pictureBox1.Image = e.Bitmap;
    }

    private void button1_Click(object sender, EventArgs e)
    {

    }
    public static string SerializeToString(object obj)
    {
      XmlSerializer serializer = new XmlSerializer(obj.GetType());

      using (StringWriter writer = new StringWriter())
      {
        serializer.Serialize(writer, obj);

        return writer.ToString();
      }
    }
  }
}
