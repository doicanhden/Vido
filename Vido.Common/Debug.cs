namespace Vido.Common
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using System.IO;

  public class Debug
  {
    private readonly static Debug instance = new Debug();
    private readonly object objLock = new object();
    private readonly StringBuilder buffer = new StringBuilder();

    #region Static Methods
    public static Debug Instance
    {
      get { return (instance); }
    }
    public static void Save(string fileName, bool clearLog = true)
    {
      lock (Instance.objLock)
      {
        File.WriteAllText(fileName, Instance.buffer.ToString());

        if (clearLog)
        {
          Instance.buffer.Clear();
        }
      }
    }

    #region Warning
    public static void Warning(int error)
    {
      Instance.ErrorHandling(ErrorLevels.Warning, error, string.Empty, string.Empty);
    }
    public static void Warning(int error, string message)
    {
      Instance.ErrorHandling(ErrorLevels.Warning, error, message, string.Empty);
    }
    public static void Warning(string message)
    {
      Instance.ErrorHandling(ErrorLevels.Warning, -1, message, string.Empty);
    }
    public static void Warning(string message, string category)
    {
      Instance.ErrorHandling(ErrorLevels.Warning, -1, message, category);
    }
    public static void Warning(int error, string message, string category)
    {
      Instance.ErrorHandling(ErrorLevels.Warning, error, message, category);
    }
    #endregion

    #region Logging
    public static void Logging(int error)
    {
      Instance.ErrorHandling(ErrorLevels.Logging, error, string.Empty, string.Empty);
    }
    public static void Logging(int error, string message)
    {
      Instance.ErrorHandling(ErrorLevels.Logging, error, message, string.Empty);
    }
    public static void Logging(string message)
    {
      Instance.ErrorHandling(ErrorLevels.Logging, -1, message, string.Empty);
    }
    public static void Logging(string message, string category)
    {
      Instance.ErrorHandling(ErrorLevels.Logging, -1, message, category);
    }
    public static void Logging(int error, string message, string category)
    {
      Instance.ErrorHandling(ErrorLevels.Logging, error, message, category);
    }
    #endregion

    #region Error
    public static void Error(int error)
    {
      Instance.ErrorHandling(ErrorLevels.Severe, error, string.Empty, string.Empty);
    }
    public static void Error(int error, string message)
    {
      Instance.ErrorHandling(ErrorLevels.Severe, error, message, string.Empty);
    }
    public static void Error(string message)
    {
      Instance.ErrorHandling(ErrorLevels.Severe, -1, message, string.Empty);
    }
    public static void Error(string message, string category)
    {
      Instance.ErrorHandling(ErrorLevels.Severe, -1, message, category);
    }
    public static void Error(int error, string message, string category)
    {
      Instance.ErrorHandling(ErrorLevels.Severe, error, message, category);
    }
    #endregion

    #endregion

    private Debug()
    {
    }

    private void ErrorHandling(ErrorLevels level, int error, string message, string category)
    {
      lock (objLock)
      {
        buffer.AppendFormat("==> {0} <== {1} ====> {2}, message: {3}",
          level, category, error, message);
      }
      switch (level)
      {
        case ErrorLevels.Severe:
          break;
        case ErrorLevels.Warning:
          break;
        case ErrorLevels.Logging:
          break;
        default:
          break;
      }


    }
  }
}
