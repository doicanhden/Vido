namespace Vido.Parking.Ui.Wpf.ViewModels
{
  using System;
  using System.Linq;
  using System.Drawing;
  using System.Diagnostics;
  using System.Windows;
  using System.Windows.Interop;
  using System.Collections.ObjectModel;
  using Vido.Capture;
  using Vido.Capture.Enums;
  using Vido.Parking.Controls;
  using System.Windows.Input;

  public class MainViewModel : Utilities.NotificationObject, IDisposable
  {
    #region Data Members
    private InputDeviceList inputDevices;
    private CaptureList captures;
    private DataCenter dataCenter;
    private Controller controller;

    private readonly Settings settings = new Settings();
    private readonly ObservableCollection<LaneViewModel> laneViewModels;
    private string status = null;
    private Window mainWindow;

    private ICommand showLaneConfigsCommand;
    private ICommand showConnectionStringEditCommand;
    #endregion

    #region Public Properties for Binding
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
#endregion

    #region Public Properties
    public Settings Settings
    {
      get { return (settings); }
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
      laneViewModels = new ObservableCollection<LaneViewModel>();

      inputDevices = new InputDeviceList(MainWindowHandle());
      captures = new CaptureList(new Factory());

      dataCenter = new DataCenter();
      dataCenter.NewMessage += dataCenter_NewMessage;

      controller = new Controller(dataCenter, dataCenter, inputDevices, captures);
      dataCenter.CurrentUserId = "TEST";
      settings.SetParking(dataCenter);
      settings.SetController(controller);
      settings.SetLaneConfigs(controller.LaneConfigs);

      GenerateLaneViewModels();
      StartAllCaptures();
    }
    #endregion

    public ICommand ShowLaneConfigsCommand
    {
      get { return (showLaneConfigsCommand ?? (showLaneConfigsCommand = new Commands.RelayCommand(ShowLaneConfigsExecute))); }
      
    }
    public ICommand ShowConnectionStringEditCommand
    {
      get { return (showConnectionStringEditCommand ?? (showConnectionStringEditCommand = new Commands.RelayCommand(ShowConnectionStringEditExecute))); }
      
    }

    private void ShowConnectionStringEditExecute(object obj)
    {
      new Views.ConnectionStringEditView()
      {
        Owner = mainWindow,
        DataContext = new ConnectionStringEditViewModel()
      }.ShowDialog();
    }
    private void ShowLaneConfigsExecute(object obj)
    {
      new Views.LaneManagementView()
      {
        Owner = mainWindow,
        DataContext = new LaneManagementViewModel(this)
      }.ShowDialog();
    }

    #region Event Handlers
    private void dataCenter_NewMessage(object sender, EventArgs e)
    {
      Status = (e as Events.NewMessageEventArgs).Message;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Bật toàn bộ các camera đang sử dụng.
    /// </summary>
    private void StartAllCaptures()
    {
      foreach (var cap in captures.Captures)
      {
        cap.Start();
      }
    }

    /// <summary>
    /// Tạo ViewModel cho từng Lane.
    /// </summary>
    private void GenerateLaneViewModels()
    {
      laneViewModels.Clear();

      controller.GenerateLanes();

      if (controller.Lanes != null)
      {
        foreach (var lane in controller.Lanes)
        {
          laneViewModels.Add(new LaneViewModel(lane));
        }
      }
    }

    private IntPtr MainWindowHandle()
    {
      return (new WindowInteropHelper(mainWindow).Handle);
    }

    #endregion

    #region Implementation of IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose managed resources
        captures.Dispose();
        dataCenter.Dispose();
      }
      // free native resources
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion
  }
}
