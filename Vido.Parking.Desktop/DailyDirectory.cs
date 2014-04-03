namespace Vido.Parking.Desktop
{
  using System;
  using System.IO;
  using System.Text;

  public class DailyDirectory : IDailyDirectory
  {
    #region Data Members
    private string formatString;
    #endregion

    #region Public Constructors
    public DailyDirectory(string formatString = @"yyyy\MM\dd")
    {
      if (Path.DirectorySeparatorChar == '\\')
      {
        this.formatString = formatString.Replace(@"\", @"\\");
      }
      else
      {
        this.formatString = formatString;
      }
    }
    #endregion

    #region Implementation of IDailyDirectory
    public string RootDirectoryName { get; set; }

    public string GetFullPath(DateTime time, string name)
    {
      var directoryName = time.ToString(formatString);

      CreateDirectory(Path.Combine(RootDirectoryName, directoryName));

      return (Path.Combine(directoryName, name));
    }
    #endregion

    #region Implementation of IFileStorage
    public Stream Open(string fileName)
    {
      var path = Path.Combine(RootDirectoryName, fileName);
      return (File.Open(path, FileMode.OpenOrCreate));
    }

    public bool Exists(string fileName)
    {
      var path = Path.Combine(RootDirectoryName, fileName);
      return (File.Exists(path));
    }

    public void Delete(string fileName)
    {
      var path = Path.Combine(RootDirectoryName, fileName);
      File.Delete(path);
    }
    #endregion

    #region Private Methods
    private bool CreateDirectory(string directoryName)
    {
      try
      {
        if (!Directory.Exists(directoryName))
        {
          Directory.CreateDirectory(directoryName);
        }

        return (true);
      }
      catch
      {
        return (false);
      }
    }
    #endregion
  }
}
