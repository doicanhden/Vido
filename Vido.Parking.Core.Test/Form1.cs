
namespace Vido.Parking.Test
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using System.Xml.Serialization;
  using Vido.Capture;
  using Vido.Capture.Events;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Controls;
  using Vido.Parking.Interfaces;
  using Vido.RawInput.Events;
  using Vido.RawInput.Interfaces;

  public partial class Form1 : Form
  {
    RFIDReader rfidReader = new RFIDReader();
    RawInput.RawInput rawInput = null;
    Lane lane = null;
    public Form1()
    {
      InitializeComponent();

      lane = new Lane()
      {
        BackCamera = new JpegStream()
        {
          Configs = new CaptureConfigs()
          {
            Source = @"http://pasteldth.dyndns.org/cgi-bin/net_jpeg.cgi?ch=2",
            Username = "admin",
            Password = "123456",
            FrameInterval = 0
          }
        },
        FrontCamera = new MJpegStream()
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

      lane.BackCamera.NewFrame += capture1_NewFrame;
      lane.FrontCamera.NewFrame += capture2_NewFrame;

      lane.FrontCamera.Start();
      lane.BackCamera.Start();
      lane.UidDevice = rfidReader;
      rawInput = new RawInput.RawInput(Handle);
      rawInput.AddMessageFilter();

      //    rawInput.Keyboard.DevicesChanged += Keyboard_DevicesChanged;
      //    rawInput.Keyboard.EnumerateDevices();
      
    }

    void Keyboard_DevicesChanged(IRawKeyboard s, DevicesChangedEventArgs e)
    {
      rfidReader.Keyboard = e.NewDevices[3];
    }


    void capture2_NewFrame(object sender, NewFrameEventArgs e)
    {
      pictureBox2.Image = e.Bitmap;
    }

    void capture1_NewFrame(object sender, NewFrameEventArgs e)
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
