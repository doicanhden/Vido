namespace Vido.Parking.Ui.ViewModels
{
  using System;
  using System.Collections.ObjectModel;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.IO;
  using System.Windows.Input;
  using System.Windows.Media.Imaging;
  using Vido.Media;
  using Vido.Media.Capture;
  using Vido.Qms;

  public class LaneViewModel : Utilities.NotificationObject, IUniqueId, IUserData
  {
    #region Data Members

    #region Text of Labels
    private string uniqueIdText = null;
    private string userDataText = null;
    private string backCamText = null;
    private string frontCamText = null;
    private string backImgText = null;
    private string frontImgText = null;
    #endregion

    private readonly Lane lane = null;

    private string message = null;
    private string laneCode = null;
    private string uniqueId = null;
    private string userData = null;
    private BitmapSource backImageSaved = null;
    private BitmapSource frontImageSaved = null;
    private BitmapSource frontImageCamera = null;
    private BitmapSource backImageCamera = null;

    private ICommand stopCommand = null;
    #endregion
    public string UniqueIdText
    {
      get { return (uniqueIdText); }
      set
      {
        uniqueIdText = value;
        RaisePropertyChanged(() => UniqueIdText);
      }
    }
    public string UserDataText
    {
      get { return (userDataText); }
      set
      {
        userDataText = value;
        RaisePropertyChanged(() => UserDataText);
      }
    }
    public string BackCamText
    {
      get { return (backCamText); }
      set
      {
        backCamText = value;
        RaisePropertyChanged(() => BackCamText);
      }
    }
    public string FrontCamText
    {
      get { return (frontCamText); }
      set
      {
        frontCamText = value;
        RaisePropertyChanged(() => FrontCamText);
      }
    }
    public string BackImgText
    {
      get { return (backImgText); }
      set
      {
        backImgText = value;
        RaisePropertyChanged(() => BackImgText);
      }
    }
    public string FrontImgText
    {
      get { return (frontImgText); }
      set
      {
        frontImgText = value;
        RaisePropertyChanged(() => FrontImgText);
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

    public string LaneCode
    {
      get { return (laneCode); }
      set
      {
        laneCode = value;
        RaisePropertyChanged(() => LaneCode);
      }
    }
    public string UniqueId
    {
      get { return (uniqueId); }
      set
      {
        uniqueId = value;
        RaisePropertyChanged(() => UniqueId);
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
    public BitmapSource BackImageCamera
    {
      get { return (backImageCamera); }
      set
      {
        backImageCamera = value;
        RaisePropertyChanged(() => BackImageCamera);
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
    public BitmapSource BackImageSaved
    {
      get { return (backImageSaved); }
      set
      {
        backImageSaved = value;
        RaisePropertyChanged(() => BackImageSaved);
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

    #region Public Construtors
    public LaneViewModel(Lane lane)
    {
      if (lane == null)
        throw new ArgumentNullException("lane");

      this.lane = lane;
      this.lane.NewMessage += lane_NewMessage;

      this.LaneCode = lane.Name;

      if (this.lane.CameraBack != null)
      {
        this.lane.CameraBack.NewFrame += BackCamera_NewFrame;
      }

      if (this.lane.CameraFront != null)
      {
        this.lane.CameraFront.NewFrame += FrontCamera_NewFrame;
      }

      /// TODO: Địa phương hóa chuỗi thông báo.
      UniqueIdText = "Thẻ xe";
      UserDataText = "Biển số";
      
      BackCamText = "Camera phía sau";
      FrontCamText = "Camera phía trước";

      BackImgText = "Ảnh chụp phía sau";
      FrontImgText = "Ảnh chụp phía sau";

      if (this.lane.Direction == Direction.Import)
      {
        this.LaneCode += " \nVào";
      }
      else
      {
        this.BackImgText += " - Lúc VÀO";
        this.FrontImgText += " - Lúc VÀO";

        this.LaneCode += " \nRa";
      }
    }
    #endregion


    #region Event Handlers

    private void lane_NewMessage(object sender, EventArgs e)
    {
      this.Message = (e as NewMessageEventArgs).Message;
    }

    private void BackCamera_NewFrame(object sender, EventArgs e)
    {
      var args = e as NewFrameEventArgs;
      var image = args.Image as BitmapImageHolder;
      if (image != null)
      {
        this.BackImageCamera = image.Image;
      }
    }

    private void FrontCamera_NewFrame(object sender, EventArgs e)
    {
      var args = e as NewFrameEventArgs;
      var image = args.Image as BitmapImageHolder;
      if (image != null)
      {
        this.FrontImageCamera = image.Image;
      }
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
      return (false);
    }
    private void StopExecute(object obj)
    {
      lane.Block.Set();
    }
    #endregion
  }
}
