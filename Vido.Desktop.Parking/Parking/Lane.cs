
namespace Vido.Parking
{
  using System;
  using System.Threading;
  using Vido.Media.Capture;
  using Vido.Qms;

  public class Lane : IGate
  {
    public string Name { get; set; }
    public IInputDevice Input { get; set; }
    public IDisposable Deregister { get; set; }
    public GateState State { get; set; }
    public Direction Direction { get; set; }
    public ICapture CameraBack { get; set; }
    public ICapture CameraFront { get; set; }
    public IPrinter Printer { get; set; }

    public EventWaitHandle Allow { get; set; }
    public EventWaitHandle Block { get; set; }

    public event EventHandler NewMessage;
    public event EventHandler SavedImage;
    public event EventHandler EntryAllowed;
    public event EventHandler EntryBlocked;

    public Lane()
    {
      Allow = new AutoResetEvent(false);
      Block = new AutoResetEvent(false);
    }

    public void StartEntryRequest(int timeout)
    {
      throw new NotImplementedException();
    }


    void IGate.RasieNewEntries(EntryArgs entryArgs)
    {
    }

    void IGate.RaiseSavedImage(IFileStorage fileStorage, string first, string second)
    {
      if (SavedImage != null)
      {
        SavedImage(this, new SavedImagesEventArgs()
        {
          FileStorage = fileStorage,
          FirstImage = first,
          SecondImage = second
        });
      }
    }

    void IGate.RaiseNewMessage(string messages)
    {
      if (NewMessage != null)
      {
        NewMessage(this, new NewMessageEventArgs()
        {
          Message = messages
        });
      }
    }

    void IGate.RaiseEntryAllow(string userData)
    {
      throw new System.NotImplementedException();
    }

    void IGate.RaiseEntryBlock(string userData)
    {
      throw new System.NotImplementedException();
    }
  }
}