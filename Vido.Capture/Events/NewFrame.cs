namespace Vido.Capture.Events
{
  using System;
  using System.Drawing;
  using Vido.Capture.Interfaces;

  public delegate void NewFrameEventHandler(ICapture sender, NewFrameEventArgs e);

  public class NewFrameEventArgs : EventArgs
  {
    private Bitmap bitmap;

    public NewFrameEventArgs(Bitmap bitmap)
    {
      this.bitmap = bitmap;
    }

    // Bitmap property
    public Bitmap Bitmap
    {
      get { return (bitmap); }
    }
  }
}
