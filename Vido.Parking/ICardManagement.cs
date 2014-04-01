namespace Vido.Parking
{
  public interface ICardManagement
  {
    #region Public Methods
    /// <summary>
    /// Kiểm tra thẻ có tồn tại và đang được sử dụng hay không.
    /// </summary>
    /// <param name="data">Dữ liệu thẻ</param>
    /// <returns></returns>
    bool IsExistAndUsing(string cardId);
    #endregion
  }
}