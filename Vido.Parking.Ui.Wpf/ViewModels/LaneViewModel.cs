namespace Vido.Parking.Ui.Wpf.ViewModels
{
  using System.Drawing;
  using System.Windows.Input;

  public class LaneViewModel : Utilities.NotificationObject
  {
    private string laneCode = null;
    private string cardID = null;
    private string userData = null;
    private string message = null;
    private ICommand stopCommand = null;
    private Image backImageSaved = null;
    private Image frontImageSaved = null;
    private Image frontImageCamera = null;
    private Image backImageCamera = null;

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
    #region Commnands
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
  }
}
