namespace Vido.Desktop
{
  using System;
  using System.Drawing;
  using System.IO;

  public class ImageHolder : IImageHolder
  {
    #region Data Members
    private readonly object locker = new object();
    private Image image = null;
    #endregion

    #region Public Constructors
    public ImageHolder()
    {
      this.image = null;
      this.Available = false;
    }

    public ImageHolder(Image image)
    {
      this.image = image;
      this.Available = (this.image != null);
    }
    #endregion

    #region Public Properties
    public bool Available { get; set; }
    #endregion

    #region Public Methods
    public IImageHolder Copy()
    {
      lock (locker)
      {
        if (Available)
        {
          return (new ImageHolder(new Bitmap(image)));
        }

        return (null);
      }
    }

    public bool Load(Stream stream)
    {
      lock (locker)
      {
        try
        {
          Available = (image = Bitmap.FromStream(stream)) != null;
          return (true);
        }
        catch
        {
          image = null;
          Available = false;
          return (false);
        }
      }
    }

    public bool Save(Stream stream)
    {
      lock (locker)
      {
        try
        {
          if (image != null)
          {
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return (true);
          }
          return (false);
        }
        catch
        {
          return (false);
        }
      }
    }
    #endregion

    #region Implementation of IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        image.Dispose();
      }
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion
  }
}
