namespace Vido.Parking
{
  using Vido.Capture;

  public class ControllerServices
  {
    public IDataCenter DataCenter { get; set; }
    public IDailyDirectory DailyDirectory { get; set; }
    public ICaptureFactory CaptureFactory { get; set; }
    public IUniqueIdDeviceList UniqueIdDeviceList { get; set; }

    public ControllerServices(
      IDataCenter dataCenter,
      IDailyDirectory dailyDirectory,
      ICaptureFactory captureFactory,
      IUniqueIdDeviceList uniqueDeviceList)
    {
      this.DataCenter = dataCenter;
      this.DailyDirectory = dailyDirectory;
      this.CaptureFactory = captureFactory;
      this.UniqueIdDeviceList = uniqueDeviceList;
    }
  }
}
