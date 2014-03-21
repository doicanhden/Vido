
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
    private CaptureFactory captureFactory = new CaptureFactory();
    private Controller controller = null;
    public Form1()
    {
      InitializeComponent();

      controller = new Controller(null, captureFactory, RFIDReaderEnumerator.GetInstance(Handle));
    }
  }
}
