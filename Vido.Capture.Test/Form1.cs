namespace Vido.Capture.Test
{
  using System.Diagnostics;
  using System.Windows.Forms;
  using Vido.Capture.Events;
  using Vido.Capture.Interfaces;
  using Vido.Common;

  public partial class Form1 : Form
  {
    private ICapture capture = null;
    public Form1()
    {
      InitializeComponent();
      // JPEG http://pasteldth.dyndns.org/cgi-bin/net_jpeg.cgi?ch=2
      // MJPEG http://64.122.208.241:8000/axis-cgi/mjpg/video.cgi?resolution=320x240
      var captureFactory = new CaptureFactory();
      captureFactory.CaptureType = Enums.Coding.Jpeg;

      capture = captureFactory.Create();
      capture.NewFrame += capture_NewFrame;

      var config = new CaptureConfigs();
      config.Source = @"http://pasteldth.dyndns.org/cgi-bin/net_jpeg.cgi?ch=2";
      config.Username = "admin";
      config.Password = "123456";
      config.FrameInterval = 0;

      capture.Configs = config;
    }


    void capture_NewFrame(ICapture sender, NewFrameEventArgs e)
    {
      pictureBox1.Image = e.Bitmap;
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
