namespace Vido.Capture.Test
{
  using System.Diagnostics;
  using System.Windows.Forms;
  using Vido.Capture.Events;
  using Vido.Capture;
  using System.IO;
  using System.Drawing;

  public partial class Form1 : Form
  {
    private ICapture capture = null;
    public Form1()
    {
      InitializeComponent();
      // JPEG http://pasteldth.dyndns.org/cgi-bin/net_jpeg.cgi?ch=2
      // MJPEG http://64.122.208.241:8000/axis-cgi/mjpg/video.cgi?resolution=320x240

      //captureFactory.CaptureType = Enums.Coding.Jpeg;

      //capture = captureFactory.Create();

      var config = new Configuration();
      config.Source = @"http://64.122.208.241:8000/axis-cgi/mjpg/video.cgi?resolution=320x240";
      config.Username = "admin";
      config.Password = "123456";
      config.FrameInterval = 0;
      config.Coding = Enums.Coding.MJpeg;

      var captureFactory = new CaptureFactory();
      capture = captureFactory.Create(config);
      capture.NewFrame += capture_NewFrame;
    }


    void capture_NewFrame(object sender, System.EventArgs e)
    {
      using (Stream stream = new MemoryStream())
      {
        (e as NewFrameEventArgs).Image.Save(stream);
        pictureBox1.Image = Bitmap.FromStream(stream);
      }
    }

    private void button1_Click(object sender, System.EventArgs e)
    {
      capture.Start();
    }

    private void button2_Click(object sender, System.EventArgs e)
    {
      capture.Stop();
    }
  }
}
