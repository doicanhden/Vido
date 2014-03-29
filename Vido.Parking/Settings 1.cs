namespace Vido.Parking
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Xml.Linq;
  using System.Xml.Serialization;
  using Vido.Parking.Events;
  using Vido.Parking;

  public class Settings : ISettingsProvider
  {
    #region Data Members
    private Dictionary<string, string> configs = new Dictionary<string, string>();
    #endregion

    #region Public Events
    public event EventHandler SettingChanged;
    #endregion

    #region Public Properties
    /// <summary>
    /// Đường dẫn tệp tin cấu hình.
    /// </summary>
    public string FileName { get; set; }
    #endregion

    #region Public Constructors

    /// <summary>
    /// Tạo đối tượng Settings
    /// </summary>
    /// <param name="fileName">Đường dẫn tệp tin cấu hình.</param>
    public Settings(string fileName)
    {
      this.FileName = fileName;
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy cấu hình từ tệp.
    /// </summary>
    public void Load()
    {
      if (File.Exists(FileName))
      {
        var xElem = XElement.Load(FileName);

        configs = xElem.Descendants("Config").ToDictionary(
          x => x.Attribute("Key").Value,
          x => x.Attribute("Value").Value);
      }
    }

    /// <summary>
    /// Lưu cấu hình xuống tệp.
    /// </summary>
    public void Save()
    {
      var xElem = new XElement("Configs",
        configs.Select(x => new XElement("Config",
          new XAttribute("Key", x.Key),
          new XAttribute("Value", x.Value))));

      xElem.Save(FileName);
    }

    /// <summary>
    /// Thêm/đặt lại cấu hình.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Khóa</param>
    /// <param name="value"></param>
    public void Set<T>(string key, T value)
    {
      configs[key] = Serialize(value);

      if (SettingChanged != null)
      {
        SettingChanged(this, new SettingChangedEventArgs(key));
      }
    }

    /// <summary>
    /// Lấy cấu hình
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng</typeparam>
    /// <param name="key">Khóa</param>
    /// <returns>Đối tượng</returns>
    public T Query<T>(string key)
    {
      if (configs.ContainsKey(key))
      {
        return (Deserialize<T>(configs[key]));
      }

      return (default(T));
    }
    #endregion

    #region Private Utilities
    private static string Serialize<T>(T toSerialize)
    {
      var text = new StringWriter();
      var serializer = new XmlSerializer(toSerialize.GetType(), string.Empty);

      serializer.Serialize(text, toSerialize);
      return (text.ToString());
    }

    private static T Deserialize<T>(string xml)
    {
      var text = new StringReader(xml);
      var serializer = new XmlSerializer(typeof(T));

      return ((T)serializer.Deserialize(text));
    }
    #endregion
  }
}
