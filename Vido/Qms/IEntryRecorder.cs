// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  public interface IEntryRecorder
  {
    #region Methods
    bool CanImport(string uniqueId, string userData);
    bool Import(Entry entry);

    bool CanExport(string uniqueId, string userData, out string first, out string second);
    bool Export(Entry entry);

    void Blocked(Entry entry);
    #endregion
  }
}