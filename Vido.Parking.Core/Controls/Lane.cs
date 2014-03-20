namespace Vido.Parking.Controls
{
  using System;
  using System.Drawing;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;

  public class Lane
  {
    private IUidDevice uidDevice = null;

    #region Public Properties
    public ICapture FrontCamera { get; set; }
    public ICapture BackCamera  { get; set; }
    public IUidDevice UidDevice
    {
      get { return (uidDevice); }
      set
      {
        if (uidDevice != null)
        {
          uidDevice.DataIn -= uidDevice_DataIn;
        }

        uidDevice = value;

        if (uidDevice != null)
        {
          uidDevice.DataIn += uidDevice_DataIn;
        }
      }
    }
    public Direction Direction { get; set; }
    public LaneState State { get; set; }
    public int NumberOfRetries { get; set; }

    public string Message { get; set; }
    #endregion

    public event EntryEventHandler Entry;

    public Lane()
    {
    }

    private void uidDevice_DataIn(object sender, DataInEventArgs e)
    {
      if (e.Data == null || Entry == null)
      {
        return;
      }

      try
      {
        Image frontImage = TryCapture(FrontCamera);
        Image backImage = null;
        if (BackCamera != null)
        {
          backImage = TryCapture(BackCamera);
        }

        Entry(this, new EntryEventArgs(e.Data, frontImage, backImage));
      }
      catch (InvalidOperationException ex)
      {

      }
    }

    private Image TryCapture(ICapture capture)
    {
      for (int i = 0; i < NumberOfRetries; ++i)
      {
        var image = capture.Take();
        if (image != null)
        {
          return (image);
        }
      }

      throw new System.InvalidOperationException("Can't capture from Device");
    }
  }
}
