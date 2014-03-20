namespace Vido.Parking.Controls
{
  using Vido.Parking.Interfaces;

  public static class Card
  {
    static public bool IsUse(IDatabase database, string cardId)
    {
      var res = database.ExecuteScalar(string.Format("Select IsUse from Card where CardID=\'{0}\'", cardId));

      int val = 0;
      if (res != null && int.TryParse(res, out val))
      {
        return (val == 1);
      }

      return (false);
    }
  }
}
