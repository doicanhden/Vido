using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vido.Capture;
using Vido.Capture.Enums;
using Vido.Parking.Interfaces;
using Vido.RawInput.Events;

namespace Vido.Parking.Test2
{
  public partial class Form1 : Form
  {
    Parking parking;
    Controller controller;
    RawInput.RawInput rawInput;
    public Form1()
    {
      InitializeComponent();

      parking = new Parking();
      parking.Settings = DefaultSetting(Application.StartupPath + @"\settings.xml");
      parking.Settings.Save();

      controller = new Controller(parking, null, null);

      rawInput = new RawInput.RawInput(Handle);
      rawInput.AddMessageFilter();
      rawInput.Keyboard.DevicesChanged += Keyboards_DevicesChanged;
      rawInput.Keyboard.EnumerateDevices();
    }
    
    private void Keyboards_DevicesChanged(object s, DevicesChangedEventArgs e)
    {
      if (e.OldDevices != null)
      {
        foreach (var keyboard in e.OldDevices)
        {
          foreach (var lane in controller.Lanes)
          {
            if (lane.UidDeviceName == keyboard.Name)
            {
              keyboard.KeyDown -= lane.keyboard_KeyDown;
            }
          }
        }
      }
      if (e.NewDevices != null)
      {
        foreach (var keyboard in e.NewDevices)
        {
          foreach (var lane in controller.Lanes)
          {
            if (lane.UidDeviceName == keyboard.Name)
            {
              keyboard.KeyDown += lane.keyboard_KeyDown;
            }
          }
        }
      }
    }

    private void keyboard_KeyDown(object sender, RawInput.Events.KeyEventArgs e)
    {
    }
    private static ISettingsProvider DefaultSetting(string fileName)
    {
      var settings = new Settings(fileName);

      settings.Set(SettingKeys.RootImageDirectoryName, Application.StartupPath + @"\Images");
      settings.Set(SettingKeys.DailyDirectoryFormat, "{0}yyyy{0}MM{0}dd");
      settings.Set(SettingKeys.BackImageNameFormat , "{0}{1}{2}{3}B.jpg");
      settings.Set(SettingKeys.FrontImageNameFormat, "{0}{1}{2}{3}F.jpg");
      settings.Set(SettingKeys.InFormat , "I");
      settings.Set(SettingKeys.OutFormat, "O");

      settings.Set(SettingKeys.Lanes, new LaneConfigs[4]
      {
        // RFID
        new LaneConfigs()
        {
          Direction = Enums.Direction.In,
          UidDeviceName = @"\\?\HID#VID_0E6A&PID_030B#6&bb84ee5&1&0000#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}",
          NumberOfRetries = 3
        },
        // PS keyboard
        new LaneConfigs()
        {
          Direction = Enums.Direction.Out,
          UidDeviceName = @"\\?\ACPI#PNP0303#4&87d93da&0#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}"
        },
        // Bluetooth
        new LaneConfigs()
        {
          Direction = Enums.Direction.Out,
          UidDeviceName = @"\\?\HID#BthHFPHID#9&3142dfdc&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}"
        },
        // Unknown
        new LaneConfigs()
        {
          Direction = Enums.Direction.Out,
          UidDeviceName = @"\\?\HID#{0000110e-0000-1000-8000-00805f9b34fb}_LOCALMFG&000a&Col01#8&223b678f&2&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}"
        }
      });

      return (settings);
    }

  }
}
