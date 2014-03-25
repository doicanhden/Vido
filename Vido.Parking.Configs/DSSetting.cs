using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vido.Parking.Interfaces;

namespace Vido.Parking.Configs
{
  public class DSSetting : ISettingsProvider 
  {

    public event Events.SettingChangedEventHandler SettingChanged;
    public readonly DSLaneConfig dsLaneConfig = null;
    #region Public Properties
    /// <summary>
    /// Đường dẫn tệp tin cấu hình.
    /// </summary>
    public string FileName { get; set; }
    #endregion
    public DSSetting()
    {
      dsLaneConfig = new DSLaneConfig();
    }    
    public void Load()
    {
      dsLaneConfig.ReadXml(FileName);
    }

    public void Save()
    {
      dsLaneConfig.WriteXml(FileName);
    }

    public void Set<T>(string key, T value)
    {
      throw new NotImplementedException();
    }

    public T Query<T>(string key)
    {
      var rows = dsLaneConfig.Tables["LaneConfig"].Rows;
      LaneConfigs[] laneCfgs = new LaneConfigs[rows.Count];

      for (int i = 0; i < rows.Count; i++)
      {
        laneCfgs[i] = new LaneConfigs()
        {
          Direction = Convert.ToInt16(rows[i]["Direction"]),
          BackCamera = new Capture.CaptureConfigs()
          {

          }

        };
      }

      return (T)Convert.ChangeType(laneCfgs, typeof( T));
    }
  }

}
