namespace Vido.Parking
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Configuration;
  using System.Drawing;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Controls;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;
  using Vido.Parking.Properties;


  public class Controller
  {
    #region Data Members
    private readonly IParking parking = null;
    private readonly ICaptureFactory captureFactory = null;
    private readonly IUidDevicesEnumerator devicesEnumlator = null;
    private readonly List<Lane> lanes = new List<Lane>();
    private IList<IUidDevice> uidDevices = null;
    #endregion

    #region Constructors
    public Controller(IParking parking, ICaptureFactory captureFactory, IUidDevicesEnumerator devicesEnumlator)
    {
      this.parking = parking;
      this.captureFactory = captureFactory;
      this.devicesEnumlator = devicesEnumlator;
      this.devicesEnumlator.DevicesChanged += devicesEnumlator_DevicesChanged;

      if (LaneConfigs == null)
      {
        LaneConfigs = new LaneConfigs[1] { new LaneConfigs() };
        SaveConfigs();
      }
      InitLanes();
    }
    #endregion
    private void InitLanes()
    {
      lanes.Clear();
      uidDevices = devicesEnumlator.GetDevicesList();

      foreach (var cfg in Settings.Default.Lanes)
      {
        var lane = new Lane()
        {
          FrontCamera = captureFactory.Create(cfg.FrontCamera),
          BackCamera = captureFactory.Create(cfg.BackCamera),
          Direction = cfg.Direction,
          NumberOfRetries = cfg.NumberOfRetries,
          State = cfg.State
        };

        if (uidDevices != null)
        {
          foreach (var i in uidDevices)
          {
            if (cfg.UidDeviceName == i.Name)
            {
              lane.UidDevice = i;
              break;
            }
          }
        }

        lane.Entry += lane_Entry;

        lanes.Add(lane);
      }
    }
    private void ClearLanes()
    {
     
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
            if (lane.UidDevice.Name == uidDev.Name)
            {
              lane.UidDevice = uidDev;
              break;
            }
          }
        }

      }
    }
    private void lane_Entry(object sender, EntryEventArgs e)
    {
      var lane = sender as Lane;

      switch (lane.Direction)
      {
        case Direction.Out:
          if (parking.CanExit(e.Uid, e.PlateNumber))
          {
            parking.Exit(e.Uid, e.PlateNumber, e.FrontImage, e.BackImage);
          }
          else
          {
            e.Allow = false;
          }
          break;
        case Direction.In:
          if (parking.CanEntry(e.Uid, e.PlateNumber))
          {
            parking.Entry(e.Uid, e.PlateNumber, e.FrontImage, e.BackImage);
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
    private static LaneConfigs[] LaneConfigs
    {
      get { return (Properties.Settings.Default.Lanes); }
      set { Properties.Settings.Default.Lanes = value; }
    }
    private static void SaveConfigs()
    {
      Properties.Settings.Default.Save();
    }
  }
}
