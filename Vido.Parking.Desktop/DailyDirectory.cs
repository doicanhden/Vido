namespace Vido.Parking.Desktop
{
  using System;
  using System.IO;
  using System.Text;

  public class DailyDirectory : IDailyDirectory
  {
    private string formatString;

    public string RootDirectoryName { get; set; }

    public DailyDirectory(string formatString = @"yyyy{0}MM{0}dd")
    {
      this.formatString = string.Format(formatString,
        Path.DirectorySeparatorChar != '\\',
        Path.DirectorySeparatorChar, @"\\");
    }

    public Stream FileNew(DateTime time, string name, out string fileName)
    {
      var sb = new StringBuilder();
      Path.Combine(RootDirectoryName, name);
      sb.Append(Path.DirectorySeparatorChar);
      sb.Append(time.ToString(formatString));

      if (CreateDirectory(RootDirectoryName + sb.ToString()))
      {
        sb.Append(Path.DirectorySeparatorChar);
        sb.Append(name);
        fileName = sb.ToString();

        return (new FileStream(RootDirectoryName + fileName, FileMode.CreateNew));
      }
      fileName = string.Empty;
      return (null);
    }

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

    public Stream Open(string fileName)
    {
      return (File.Open(Path.Combine(RootDirectoryName, fileName), FileMode.Open));
    }

    public bool Exists(string fileName)
    {
      return (File.Exists(Path.Combine(RootDirectoryName, fileName)));
    }

    public void Delete(string fileName)
    {
      File.Delete(Path.Combine(RootDirectoryName, fileName));
    }
  }
}
