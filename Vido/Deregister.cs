// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido
{
  using System;

  public class Deregister<T> : IDisposable
    where T: class
  {
    private readonly Action<T> deregister;
    private readonly T obj;

    public Deregister(T obj, Action<T> deregister)
    {
      this.obj = obj;
      this.deregister = deregister;
    }

    public void Dispose()
    {
      if (deregister != null)
      {
        deregister(obj);
      }
    }
  }
}
