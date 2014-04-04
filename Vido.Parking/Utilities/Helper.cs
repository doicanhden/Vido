namespace Vido.Parking.Utilities
{
  using System.Linq;

  public static class Helper
  {
    public static bool TestDBConnection()
    {
      try
      {
        Vido.Parking.VidoParkingEntities entities = new VidoParkingEntities();


        return (entities.Database.Exists());
      }
      catch
      {
        return (false);
      }
    }
  }
}
