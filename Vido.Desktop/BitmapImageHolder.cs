namespace Vido.Desktop
{
  using System;
  using System.Drawing;
  using System.IO;

  public class BitmapImageHolder : IImageHolder
  {
    #region Data Members
    private readonly object locker = new object();
    private Image image = null;
    #endregion

    #region Public Constructors
    public BitmapImageHolder()
    {
      this.image = null;
    }

    public BitmapImageHolder(Image image)
    {
      this.image = image;
    }
    #endregion

    #region Public Properties
    public bool Available
    {
      get
      {
        lock (locker)
        {
          return (this.image != null);
        }
      }
    }
    #endregion

    #region Public Methods
    public IImageHolder Copy()
    {
      lock (locker)
      {
        if (image != null)
        {
          return (new BitmapImageHolder(new Bitmap(image)));
        }

        return (null);
      }
    }

    public bool Load(IFileStorage storage, string fileName)
    {
      using (var stream = storage.Open(fileName))
      {
        return (Load(stream));
      }
    }
    public bool Save(IFileStorage storage, string fileName)
    {
      using (var stream = storage.Open(fileName))
      {
        return (Save(stream));
      }
    }

    public bool Load(Stream stream)
    {
      lock (locker)
      {
        try
        {
          image = Bitmap.FromStream(stream);
          return (true);
        }
        catch
        {
          image = null;
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
