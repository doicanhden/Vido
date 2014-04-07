// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;
  using System.Threading;
  using Vido.Capture;

  public interface IGate
  {
    #region Properties
    string Name { get; set; }
    GateState State { get; set; }
    Direction Direction { get; set; }

    EventWaitHandle Allow { get; set; }
    EventWaitHandle Block { get; set; }

    ICapture CameraBack { get; set; }
    ICapture CameraFront { get; set; }
    IPrinter Printer { get; set; }
    #endregion

    #region Events
    event EventHandler NewMessage;
    event EventHandler SavedImage;
    event EventHandler EntryAllowed;
    event EventHandler EntryBlocked;
    #endregion

    #region Methods
    void NewEntries(IUniqueId uniqueId, IUserData userData);

    void RaiseSavedImage(IFileStorage fileStorage, string first, string second);
    void RaiseNewMessage(string messages);
    void RaiseEntryAllow(string userData);
    void RaiseEntryBlock(string userData);
    #endregion
  }
}