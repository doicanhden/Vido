namespace Vido.Parking
{
  using Autofac;
  using System;

  public class Factories
  {
    private static IContainer current;

    public static bool Available
    {
      get { return (current != null); }
    }

    public static void ThrowExceptionIfCurrentNotSet()
    {
      if (current == null)
      {
        throw new MemberAccessException("Current Factory is not set");
      }
    }

    public static IContainer Current
    {
      get { return (current); }
      set { current = value; }
    }
  }
}
