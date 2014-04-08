// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;

  public interface IEntryReporter
  {
    IDisposable Register(IGate gate);
  }
}