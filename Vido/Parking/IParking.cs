namespace Vido.Parking
{
  using Vido.Qms;

  public interface IParking : IEntryRecorder
  {
    #region Public Properties
    /// <summary>
    /// Số lượng chỗ đỗ tối đa của Bãi.
    /// </summary>
    int MaximumSlots { get; set; }

    /// <summary>
    /// Trạng thái Bãi đầy.
    /// </summary>
    bool IsFull { get; }
    #endregion
 }
}