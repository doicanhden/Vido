namespace Vido.Parking.Controls
{
  using System;
  using System.Drawing;
  using System.Threading.Tasks;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;
  using Vido.Parking.Utilities;

  public class Lane
  {
    #region Data Members
    private bool isStarted = false;
    private IUidDevice uidDevice = null;
    #endregion

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

    #region Public Events
    public event EntryEventHandler Entry;
    public event EntryAllowedEventHandler EntryAllowed;
    #endregion

    #region Constructors
    public Lane()
    {
    }
    #endregion

    #region Public Methods
    public bool Start()
    {
      bool ret = false;
      if (State == LaneState.Ready)
      {
        if (BackCamera != null)
        {
          ret = BackCamera.Start();
        }

        if (ret && FrontCamera != null)
        {
          ret = FrontCamera.Start();
        }

        isStarted = ret;
      }

      return (ret);
    }
    public void Stop()
    {
      isStarted = false;
    }
    #endregion

    #region Event Handlers
    private void uidDevice_DataIn(object sender, DataInEventArgs e)
    {
      if (!isStarted || e.Data == null || Entry == null || State == LaneState.Stop)
      {
        return;
      }

      try
      {
        Image backImage = TryCapture(BackCamera);
        Image frontImage = null;
        if (FrontCamera != null)
        {
          frontImage = TryCapture(FrontCamera);
        }

//      Task task = new Task(() =>
//      {
        var entry = new EntryEventArgs(e.Data, Ocr.GetPlateNumber(frontImage), frontImage, backImage);

        Entry(this, entry);

        if (entry.Allow && EntryAllowed != null)
        {
          EntryAllowed(this, new EntryAllowedEventArgs(entry.PlateNumber));
        }
//      });

//      task.Start();
      }
      catch (InvalidOperationException ex)
      {
        throw;
      }
    }
    #endregion

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
