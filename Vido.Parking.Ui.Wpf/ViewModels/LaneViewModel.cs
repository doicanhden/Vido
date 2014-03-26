namespace Vido.Parking.Ui.Wpf.ViewModels
{
  using System;
  using System.Drawing;
  using System.Windows.Input;
  using Vido.Parking.Controls;

  public class LaneViewModel : Utilities.NotificationObject
  {
    #region Data Members
    private readonly Lane lane = null;

    private string laneCode = null;
    private string cardID = null;
    private string userData = null;
    private string message = null;
    private Image backImageSaved = null;
    private Image frontImageSaved = null;
    private Image frontImageCamera = null;
    private Image backImageCamera = null;
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

    public Image FrontImageCamera
    {
      get { return (frontImageCamera); }
      set
      {
        frontImageCamera = value;
        RaisePropertyChanged(() => FrontImageCamera);
      }
    }
    public Image BackImageCamera
    {
      get { return (backImageCamera); }
      set
      {
        backImageCamera = value;
        RaisePropertyChanged(() => BackImageCamera);
      }
    }

    public Image FrontImageSaved
    {
      get { return (frontImageSaved); }
      set
      {
        frontImageSaved = value;
        RaisePropertyChanged(() => FrontImageSaved);
      }
    }
    public Image BackImageSaved
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
      this.CardID = Controller.EncodeData(e.Data);
      this.UserData = e.PlateNumber;
    }

    private void lane_LastImages(object sender, Events.LastImagesEventArgs e)
    {
      if (e.Back != null)
      {
        this.BackImageCamera = new Bitmap(e.Back);
      }

      if (e.Front != null)
      {
        this.FrontImageSaved = new Bitmap(e.Front);
      }
    }

    private void lane_NewMessage(object sender, Events.NewMessageEventArgs e)
    {
      this.Message = e.Message;
    }

    private void BackCamera_NewFrame(object sender, Capture.Events.NewFrameEventArgs e)
    {
      this.BackImageCamera = new Bitmap(e.Bitmap);
    }

    private void FrontCamera_NewFrame(object sender, Capture.Events.NewFrameEventArgs e)
    {
      this.FrontImageCamera = new Bitmap(e.Bitmap);
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
      var button = obj as System.Windows.Controls.Button;
      button.Content = "Xì tốp";
    }
    #endregion
  }
}
