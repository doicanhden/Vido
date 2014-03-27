namespace Vido.Parking.Ui.Wpf.ViewModels
{
  using System;
  using System.Drawing;
  using System.IO;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using System.Windows.Media.Imaging;
  using Vido.Parking.Controls;
  using Vido.Parking.Utilities;

  public class LaneViewModel : Utilities.NotificationObject
  {
    #region Data Members
    private readonly Lane lane = null;

    private string laneCode = null;
    private string cardID = null;
    private string userData = null;
    private string message = null;
    private BitmapSource backImageSaved = null;
    private BitmapSource frontImageSaved = null;
    private BitmapSource frontImageCamera = null;
    private BitmapSource backImageCamera = null;
    private ICommand stopCommand = null;
    #endregion

    public LaneViewModel(Lane lane)
    {
      if (lane == null)
        throw new ArgumentNullException("lane");

      this.lane = lane;
      this.lane.Entry += lane_Entry;
      this.lane.LastImages += lane_LastImages;
      this.lane.NewMessage += lane_NewMessage;

      this.LaneCode = lane.LaneCode;

      if (this.lane.BackCamera != null)
      {
        this.lane.BackCamera.NewFrame += BackCamera_NewFrame;
      }

      if (this.lane.FrontCamera != null)
      {
        this.lane.FrontCamera.NewFrame += FrontCamera_NewFrame;
      }
    }


    public string LaneCode
    {
      get { return (laneCode); }
      set
      {
        laneCode = value;
        RaisePropertyChanged(() => LaneCode);
      }
    }
    public string CardID
    {
      get { return (cardID); }
      set
      {
        cardID = value;
        RaisePropertyChanged(() => CardID);
      }
    }
    public string UserData
    {
      get { return (userData); }
      set
      {
        userData = value;
        RaisePropertyChanged(() => UserData);
      }
    }
    public string Message
    {
      get { return (message); }
      set
      {
        message = value;
        RaisePropertyChanged(() => Message);
      }
    }

    public BitmapSource FrontImageCamera
    {
      get { return (frontImageCamera); }
      set
      {
        frontImageCamera = value;
        RaisePropertyChanged(() => FrontImageCamera);
      }
    }
    public BitmapSource BackImageCamera
    {
      get { return (backImageCamera); }
      set
      {
        backImageCamera = value;
        RaisePropertyChanged(() => BackImageCamera);
      }
    }

    public BitmapSource FrontImageSaved
    {
      get { return (frontImageSaved); }
      set
      {
        frontImageSaved = value;
        RaisePropertyChanged(() => FrontImageSaved);
      }
    }
    public BitmapSource BackImageSaved
    {
      get { return (backImageSaved); }
      set
      {
        backImageSaved = value;
        RaisePropertyChanged(() => BackImageSaved);
      }
    }

    #region Event Handlers
    private void lane_Entry(object sender, Events.EntryEventArgs e)
    {
      this.CardID = Encode.EncodeData(e.Data);
      this.UserData = e.PlateNumber;
    }

    private void lane_LastImages(object sender, Events.LastImagesEventArgs e)
    {
      if (e.Back != null)
      {
        this.BackImageSaved = ConvertToBitmapSource(e.Back);
      }

      if (e.Front != null)
      {
        this.FrontImageSaved = ConvertToBitmapSource(e.Front);
      }
    }

    private void lane_NewMessage(object sender, Events.NewMessageEventArgs e)
    {
      this.Message = e.Message;
    }

    private void BackCamera_NewFrame(object sender, Capture.Events.NewFrameEventArgs e)
    {
      this.BackImageCamera = ConvertToBitmapSource(e.Bitmap);
    }

    private void FrontCamera_NewFrame(object sender, Capture.Events.NewFrameEventArgs e)
    {
      this.FrontImageCamera = ConvertToBitmapSource(e.Bitmap);
    }
    #endregion

    #region Stop Commnand
    public ICommand StopCommand
    {
      get
      {
        return (stopCommand ?? (stopCommand = new Commands.RelayCommand<object>(StopExecute, StopCanExecute)));
      }
    }
    private bool StopCanExecute(object obj)
    {
      return (true);
    }
    private void StopExecute(object obj)
    {
    }
    #endregion

    private static BitmapSource ConvertToBitmapSource(Image image)
    {
      if (image != null)
      {
        MemoryStream ms = new MemoryStream();
        image.Save(ms, image.RawFormat);
        ms.Seek(0, SeekOrigin.Begin);

        BitmapImage bi = new BitmapImage();
        bi.BeginInit();
        bi.StreamSource = ms;
        bi.EndInit();
        bi.Freeze();
        return (bi);
      }
      return (null);
    }
  }
}
