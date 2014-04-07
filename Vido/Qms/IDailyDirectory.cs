// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  public interface IDailyDirectory : IFileStorage
  {
    string RootDirectoryName { get; set; }
    string GetPath(System.DateTime time, string fileName);
  }
}
