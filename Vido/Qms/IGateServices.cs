// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System.Collections.Generic;

  public interface IGateServices
  {
    ICollection<IGate> GetAllGates(GateController controller);
  }
}
