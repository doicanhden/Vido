namespace Vido.Parking.Utilities
{
  using System;

  public static class Requests
  {
    public static void NotNull(object obj, string name)
    {
      if (obj == null)
      {
        throw new ArgumentNullException(name);
      }
    }
  }
}
