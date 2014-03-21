namespace Vido.Parking
{
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Controls;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;
  using Vido.Parking.Models;


  public class Controller
  {
    #region Data Members
    private readonly IParking parking = null;
    private readonly ISettingsProvider settingsProvider = null;
    private readonly ICaptureFactory captureFactory = null;
    private readonly IUidDevicesEnumerator devicesEnumlator = null;
    private readonly List<Lane> lanes = new List<Lane>();
    private IList<IUidDevice> uidDevices = null;
    #endregion

    #region Constructors
    public Controller(IParking parking, ISettingsProvider settingsProvider, ICaptureFactory captureFactory, IUidDevicesEnumerator devicesEnumlator)
    {
      this.parking = parking;
      this.settingsProvider = settingsProvider;
      this.captureFactory = captureFactory;
      this.devicesEnumlator = devicesEnumlator;
    }
    #endregion
    private void InitializeObject()
    {
      lanes.Clear();
      uidDevices = devicesEnumlator.GetDevicesList();

      var laneSetting = settingsProvider.Query<LaneConfigs[]>("Lanes");
      foreach (var cfg in laneSetting)
      {
        var lane = new Lane()
        {
          FrontCamera = captureFactory.Create(cfg.FrontCamera),
          BackCamera = captureFactory.Create(cfg.BackCamera),
          Direction = cfg.Direction,
          NumberOfRetries = cfg.NumberOfRetries,
          State = cfg.State
        };
        
        foreach (var i in uidDevices)
        {
          if (cfg.UidDeviceName == i.Name)
          {
            lane.UidDevice = i;
            break;
          }
        }

        if (lane.UidDevice == null)
        {
          lane.Message = "";
        }

        lane.Entry += lane_Entry;

        lanes.Add(lane);
      }
    }
    private void devicesEnumlator_DevicesChanged(object sender, DevicesChangedEventArgs e)
    {
      uidDevices = devicesEnumlator.GetDevicesList();
      if (lanes.Count > 0)
      {
        foreach (var lane in lanes)
        {
          foreach (var uidDev in uidDevices)
          {

          }
        }

      }
    }

    private void lane_Entry(object sender, EntryEventArgs e)
    {
      var lane = sender as Lane;
      var plateNumber = GetPlateNumber(e.FrontImage);

      switch (lane.Direction)
      {
        case Direction.Out:
          if (parking.CanExit(e.Uid, plateNumber))
          {
            parking.Exit(e.Uid, plateNumber, e.FrontImage, e.BackImage);
          }
          else
          {
            e.Allow = false;
          }
          break;
        case Direction.In:
          if (parking.CanEntry(e.Uid, plateNumber))
          {
            parking.Entry(e.Uid, plateNumber, e.FrontImage, e.BackImage);
          }
          else
          {
            e.Allow = false;
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
