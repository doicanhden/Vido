namespace Vido.Media
{
  using System.IO;
  using System.Windows.Media.Imaging;

  public class BitmapImageHolder : IImageHolder
  {
    #region Data Members
    private readonly object locker = new object();
    private BitmapImage image = null;
    #endregion

    #region Public Constructors
    public BitmapImageHolder()
    {
      this.image = new BitmapImage();
    }

    public BitmapImageHolder(BitmapImage image)
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

    public BitmapImage Image
    {
      get
      {
        lock (locker)
        {
          return (image);
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
          var stream = new MemoryStream();
          this.Save(stream);

          var copy = new BitmapImageHolder();
          copy.Load(stream);

          return (copy);
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
          if (image == null)
          {
            image = new BitmapImage();
          }
          image.BeginInit();
          image.StreamSource = stream;
          image.EndInit();
          image.Freeze();

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
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);
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
  }
}
