namespace Vido.Parking
{
  public static class SettingKeys
  {
    public const string RootImageDirectoryName = "RootImageDirectoryName";
    public const string Lanes = "Lanes";
    public const string Captures = "Captures";

    public const string InFormat = "InFormat";
    public const string OutFormat = "OutFormat";

    /// <summary>
    /// {0} - Directory separator char,
    /// yyyy - year,
    /// MM - month,
    /// dd - day
    /// </summary>
    public const string DailyDirectoryFormat = "DailyDirectoryNameFormat";

    /// <summary>
    /// {0} - Time,
    /// {1} - In/Out,
    /// {2} - Uid data,
    /// {3} - Plate number,
    /// </summary>
    public const string FrontImageNameFormat = "FrontImageNameFormat";

    /// <summary>
    /// {0} - Time,
    /// {1} - In/Out,
    /// {2} - Uid data,
    /// {3} - Plate number,
    /// </summary>
    public const string BackImageNameFormat = "BackImageNameFormat";
  }
}