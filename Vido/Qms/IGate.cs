// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;
  using System.Threading;
  using Vido.Media;
  using Vido.Media.Capture;

  public interface IGate
  {
    #region Properties
    string Name { get; set; }
    GateState State { get; set; }
    Direction Direction { get; set; }
    IDisposable Deregister { get; set; }
    EventWaitHandle Allow { get; set; }
    EventWaitHandle Block { get; set; }

    IInputDevice Input { get; set; }
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
    void RasieNewEntries(EntryArgs entryArgs);

    void RaiseSavedImage(IFileStorage fileStorage, string first, string second);
    void RaiseNewMessage(string messages);
    void RaiseEntryAllow(string userData);
    void RaiseEntryBlock(string userData);
    void StartEntryRequest(int timeout);
    #endregion
  }

  public static class IGateExtensions
  {
    public static ImagePair CaptureImages(this IGate gate)
    {
      IImageHolder back = null, front = null;
      if (gate.CameraBack != null)
      {
        back = gate.CameraBack.Take();
      }

      if (gate.CameraFront != null)
      {
        front = gate.CameraFront.Take();
      }

      if (back != null && back.Available &&
        front != null && front.Available)
      {
        return (new ImagePair()
        {
          First = back,
          Second = front
        });
      }

      return (null);
    }
  }
}