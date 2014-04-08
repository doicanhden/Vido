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

  public class MainViewModel : Utilities.NotificationObject, IDisposable
  {
    #region Data Members
    private CaptureList captures;
    private readonly ObservableCollection<LaneViewModel> laneViewModels;
    private string status = null;
    private Window mainWindow;
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
      laneViewModels = new ObservableCollection<LaneViewModel>();

      captures = new CaptureList(new CaptureFactory());

      GenerateLaneViewModels();
      StartAllCaptures();
    }
    #endregion

    #region Event Handlers
    private void dataCenter_NewMessage(object sender, EventArgs e)
    {
      Status = (e as NewMessageEventArgs).Message;
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
