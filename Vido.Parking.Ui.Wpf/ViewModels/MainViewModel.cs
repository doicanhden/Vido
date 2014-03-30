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


  public class MainViewModel : IDisposable
  {
    private InputDeviceList inputDevices;
    private CaptureList captures;
    private DataCenter dataCenter;
    private Controller controller;

    private readonly Settings settings = new Settings();
    private readonly ObservableCollection<LaneViewModel> laneViewModels;

    /// <summary>
    /// Danh sách các ViewModel của lane.
    /// </summary>
    public ObservableCollection<LaneViewModel> Lanes
    {
      get { return (laneViewModels); }
    }

    public MainViewModel(IntPtr mainWindowsHandle)
    {
      laneViewModels = new ObservableCollection<LaneViewModel>();

      inputDevices = new InputDeviceList(mainWindowsHandle);
      captures = new CaptureList(new Factory());

      dataCenter = new DataCenter();
      controller = new Controller(dataCenter, dataCenter, inputDevices, captures);
      dataCenter.CurrentUserId = "TEST";
      settings.SetParking(dataCenter);
      settings.SetController(controller);
      settings.SetLaneConfigs(controller.LaneConfigs);

      GenerateLaneViewModels();
      StartAllCaptures();
    }
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
