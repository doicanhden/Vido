namespace Vido.Parking
{
  using System;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Configuration;
  using System.Drawing;
  using Vido.Capture.Interfaces;
  using Vido.Parking.Controls;
  using Vido.Parking.Enums;
  using Vido.Parking.Events;
  using Vido.Parking.Interfaces;

  public class Controller
  {
    #region Data Members
    private readonly IParking parking = null;
    private readonly ICaptureFactory captureFactory = null;
    private readonly IUidDeviceList devicesEnumlator = null;
    private readonly ObservableCollection<Lane> lanes = new ObservableCollection<Lane>();
    #endregion

    #region Constructors
    public Controller(IParking parking, ICaptureFactory captureFactory, IUidDeviceList devicesEnumlator)
    {
      this.parking = parking;
      this.captureFactory = captureFactory;
      this.devicesEnumlator = devicesEnumlator;


      this.parking.Settings.SettingChanged += Settings_SettingChanged;
      if (LaneConfigs == null)
      {
        LaneConfigs = new LaneConfigs[1] { new LaneConfigs() };
        SaveConfigs();
      }
      InitLanes();
    }

    #endregion

    #region Public Properties
    public ObservableCollection<Lane> Lanes
    {
      get { return (lanes); }
    }
    #endregion

    #region Event Handlers
    private void Settings_SettingChanged(object sender, SettingChangedEventArgs e)
    {
      switch (e.SettingKey)
      {
        case SettingKeys.Lanes:
          InitLanes();
          break;
        default:
          break;
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
    #endregion

    private void InitLanes()
    {
      if (lanes.Count > 0)
      {
        for (int i = 0; i < lanes.Count; ++i)
        {
          devicesEnumlator.Unregister(lanes[i].UidDevice);
        }

        lanes.Clear();
      }

      if (LaneConfigs != null)
      {
        foreach (var cfg in LaneConfigs)
        {
          var lane = new Lane()
          {
            //COMMENTED FOR TESTING
            FrontCamera = captureFactory.Create(cfg.FrontCamera),
            BackCamera = captureFactory.Create(cfg.BackCamera),
            Direction = cfg.Direction,
            NumberOfRetries = cfg.NumberOfRetries,
            State = cfg.State,
            UidDevice = devicesEnumlator.Register(cfg.UidDeviceName)
          };

          lane.Entry += lane_Entry;
          lane.Start();

          lanes.Add(lane);
        }
      }
    }

    #region Settings Accessor
    private LaneConfigs[] LaneConfigs
    {
      get { return (parking.Settings.Query<LaneConfigs[]>(SettingKeys.Lanes)); }
      set { parking.Settings.Set(SettingKeys.Lanes, value); }
    }

    private void SaveConfigs()
    {
      parking.Settings.Save();
    }
    #endregion
  }
}
