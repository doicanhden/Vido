namespace Vido.Parking
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Controls;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;


  public class Controller
  {
    #region Data Members
    private readonly List<Lane> lanes = new List<Lane>();
    private readonly IUidDevicesEnumerator devicesEnumlator = null;
    private readonly IParking parking = null;
    private IList<IUidDevice> uidDevices = null;
    #endregion

    #region Constructors
    public Controller(IParking parking, ISettingsProvider settingsProvider, ICaptureFactory captureFactory, IUidDevicesEnumerator devicesEnumlator)
    {
      this.devicesEnumlator = devicesEnumlator;
      this.devicesEnumlator.DevicesChanged += devicesEnumlator_DevicesChanged;


      foreach (var lane in lanes)
      {
        lane.Entry += lane_Entry;
      }
    }
    #endregion

    private void devicesEnumlator_DevicesChanged(object sender, DevicesChangedEventArgs e)
    {
      uidDevices = devicesEnumlator.GetDevicesList();
    }

    private void lane_Entry(object s, EntryEventArgs e)
    {
      var plateNumber = GetPlateNumber(e.FrontImage);

      switch ((s as Lane).Direction)
      {
        case Vido.Parking.Enums.Direction.In:
          if (parking.CanExit(e.Uid, plateNumber))
          {
            parking.Exit(e.Uid, plateNumber, e.FrontImage, e.BackImage);
          }
          else
          {
            //
          }
          break;
        case Vido.Parking.Enums.Direction.Out:
          if (parking.CanEntry(e.Uid, plateNumber))
          {
            parking.Entry(e.Uid, plateNumber, e.FrontImage, e.BackImage);
          }
          else
          {
            //
          }
          break;
        default:
          break;
      }
    }

    private static string GetPlateNumber(Image image)
    {
      return (string.Empty);
    }
  }
}
