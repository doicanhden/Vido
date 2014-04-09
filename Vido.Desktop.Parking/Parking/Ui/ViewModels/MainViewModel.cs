namespace Vido.Parking.Ui.ViewModels
{
  using System;
  using System.Linq;
  using System.Drawing;
  using System.Diagnostics;
  using System.Windows;
  using System.Windows.Interop;
  using System.Collections.ObjectModel;
  using Vido.Media.Capture;
  using Vido.Qms;

  public class MainViewModel : Utilities.NotificationObject
  {
    #region Data Members
    private readonly Settings settings = new Settings();
    private readonly Window mainWindow;
    private readonly CaptureFactory capFactory;
    private readonly ObservableCollection<LaneViewModel> laneViewModels;
    private string status;
    #endregion

    #region Public Properties
    /// <summary>
    /// Danh sách các ViewModel của lane.
    /// </summary>
    public ObservableCollection<LaneViewModel> Lanes
    {
      get { return (laneViewModels); }
    }

    /// <summary>
    /// Trạng thái.
    /// </summary>
    public string Status
    {
      get { return (status); }
      set
      {
        status = value;
        RaisePropertyChanged(() => Status);
      }
    }

    public Window View
    {
      get { return (mainWindow); }
    }
    #endregion

    #region Public Constructors
    public MainViewModel(Window mainWindow)
    {
      this.mainWindow = mainWindow;
      this.capFactory = new CaptureFactory();
      this.laneViewModels = new ObservableCollection<LaneViewModel>();

      CenterUnit.Current.RegisterDependencies(GetHandle(mainWindow), capFactory);

      CenterUnit.Current.Recorder.NewMessage += UpdateStatus;
      CenterUnit.Current.IdStorage.NewMessage += UpdateStatus;

      BuildLane();
    }

    #endregion
    private void BuildLane()
    {
      foreach (Datasets.Settings.ParkingConfigsRow cfg in settings.Parking.Rows)
      {
        if (cfg.Level == 0)
        {
          CenterUnit.Current.Recorder.MaximumSlots = cfg.MaximumSlots;
          CenterUnit.Current.Recorder.MinimumSlots = cfg.MinimumSlots;
          break;
        }
      }

      foreach (Datasets.Settings.ControllerConfigsRow cfg in settings.Controller.Rows)
      {
        if (cfg.Id == 0)
        {
          CenterUnit.Current.ImageRoot.RootDirectoryName = cfg.ImageRootDirectoryName;
          break;
        }
      }

      foreach (Datasets.Settings.LaneConfigsRow cfg in settings.LaneConfigs.Rows)
      {
        Lanes.Add(new LaneViewModel(new InputDevice()
        {
          Name = cfg.UIdDeviceName,
          EndKey = 13
        })
        {
          Name = cfg.Code,
          Direction = (Direction)cfg.Direction,
          State = (GateState)cfg.State,

          CameraFirst = CenterUnit.Current.CaptureList.Create(new Configuration()
          {
            Source = cfg.BackCamSource,
            Coding = (Coding)cfg.BackCamCoding,
            Username = cfg.BackCamUsername,
            Password = cfg.BackCamPassword,
            FrameInterval = 100
          }),
          CameraSecond = CenterUnit.Current.CaptureList.Create(new Configuration()
          {
            Source = cfg.BackCamSource,
            Coding = (Coding)cfg.BackCamCoding,
            Username = cfg.BackCamUsername,
            Password = cfg.BackCamPassword,
            FrameInterval = 100
          })
        });
      }

      foreach (var cap in CenterUnit.Current.CaptureList.Captures)
      {
        cap.Start();
      }
    }
    private void UpdateStatus(object sender, EventArgs e)
    {
      var args = e as NewMessageEventArgs;
      Status = args.Message;
    }

    private static IntPtr GetHandle(Window window)
    {
      if (window == null)
      {
        return (IntPtr.Zero);
      }

      return (new WindowInteropHelper(window).Handle);
    }
  }
}
