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
      LaneConfigs[] laneCfgs = new LaneConfigs[dsLaneConfig.Tables["LaneConfig"].Rows.Count];
      return laneCfgs;
      
    }
  }
}
