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
    ICapture CameraFirst { get; set; }
    ICapture CameraSecond { get; set; }
    IPrinter Printer { get; set; }
    #endregion

    #region Methods
    void SavedImage(IFileStorage fileStorage, string firstPath, string secondPath);
    void NewMessage(string messages);
    void NewEntries(EntryArgs entryArgs);
    void EntryAllow(string userData);
    void EntryBlock(string userData);
    #endregion
  }

  public static class IGateExtensions
  {
    public static ImagePair CaptureImages(this IGate gate)
    {
      IImageHolder back = null, front = null;
      if (gate.CameraFirst != null)
      {
        back = gate.CameraFirst.Take();
      }

      if (gate.CameraSecond != null)
      {
        front = gate.CameraSecond.Take();
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