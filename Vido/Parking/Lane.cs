
namespace Vido.Parking
{
  using System;
  using System.Threading;
  using Vido.Capture;
  using Vido.Qms;

  public class LaneController : IGate
  {
    public IInputDevice InputDevice { get; set; }
    public IUserData UserData { get; set; }
    public string Name { get; set; }
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

    public LaneController()
    {
      Allow = new AutoResetEvent(false);
      Block = new AutoResetEvent(false);
      UserData = new UserData();
    }

    public void NewEntries(IUniqueId uniqueId, IUserData userData)
    {
      throw new System.NotImplementedException();
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