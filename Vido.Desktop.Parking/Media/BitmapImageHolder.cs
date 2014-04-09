﻿namespace Vido.Media
{
  using System.Windows.Media.Imaging;
  using System.IO;
  using System;
  using System.Diagnostics;

  public class BitmapImageHolder : IImageHolder
  {
    #region Data Members
    private readonly object locker = new object();
    private BitmapImage image = null;
    #endregion

    #region Public Constructors
    public BitmapImageHolder()
    {
      this.image = null;
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
        if (image == null)
          return (null);
      }

      var copy = new BitmapImageHolder();

      var stream = new MemoryStream();
      if (this.Save(stream) && copy.Load(stream))
      {
        return (copy);
      }

      return (null);
    }

    public bool Load(IFileStorage storage, string fileName)
    {
      var stream = storage.Open(fileName);
      return (Load(stream));
    }

    public bool Load(Stream stream)
    {
      lock (locker)
      {
        try
        {
          stream.Seek(0, SeekOrigin.Begin);

          image = new BitmapImage();
          image.BeginInit();
          image.StreamSource = stream;
          image.EndInit();
          image.Freeze();

          return (true);
        }
        catch (Exception ex)
        {
          Debug.WriteLine("BitmapImageHolder.Load(Stream): " + ex.Message);
          image = null;
          return (false);
        }
      }
    }

    public bool Save(IFileStorage storage, string fileName)
    {
      var stream = storage.Open(fileName);
      return (Save(stream));
    }

    public bool Save(Stream stream)
    {
      lock (locker)
      {
        try
        {
          if (image != null)
          {
            JpegBitmapEncoder encode = new JpegBitmapEncoder();
            encode.Frames.Add(BitmapFrame.Create(image));
            encode.Save(stream);
            return (true);
          }

          return (false);
        }
        catch (Exception ex)
        {
          Debug.WriteLine("BitmapImageHolder.Save(Stream): " + ex.Message);
          return (false);
        }
      }
    }
    #endregion
  }
}
