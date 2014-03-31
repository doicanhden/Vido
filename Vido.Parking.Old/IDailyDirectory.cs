namespace Vido.Parking
{
  using System.IO;

  public interface IDailyDirectory
  {

    Stream NewFile(string fileName);
  }
}
