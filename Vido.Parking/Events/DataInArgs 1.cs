namespace Vido.Parking.Events
{
  using System;

  public class DataInEventArgs : EventArgs
  {
    #region Public Properties
    /// <summary>
    /// Dữ liệu.
    /// </summary>
    public byte[] Data { get; private set; }

    /// <summary>
    /// Dữ liệu dạng 'Có thể in'?
    /// </summary>
    public bool Printable { get; private set; }
    #endregion

    #region Public Constructors
    public DataInEventArgs(byte[] data, bool printable = false)
    {
      this.Data = data;
      this.Printable = printable;
    }
    #endregion
  }
}
