// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  public interface IEntryRecorder
  {
    #region Properties
    int MinimumSlots { get; set; }
    int MaximumSlots { get; set; }
    bool IsFull { get; }
    #endregion

    #region Methods
    bool CanImport(string uniqueId, string userData);
    bool Import(Entry entry);

    bool CanExport(string uniqueId, string userData, out string firstImagePath, out string secondImagePath);
    bool Export(Entry entry);

    void Blocked(Entry entry);
    #endregion
  }
}