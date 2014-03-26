namespace Vido.Capture
{
  using System;
  using System.Drawing;
  using System.IO;
  using System.Net;
  using System.Threading;
  using Vido.Capture.Events;
  using Vido.Capture.Interfaces;
  using Vido.Common;

  public class JpegStream : ICapture
  {
    #region Data Members
    private const int readSize = 1024;
    private const int bufSize = 512 * 1024;

    private readonly object objLock = new object();
    private readonly ManualResetEvent stopEvent = new ManualResetEvent(false);
    private Thread thread = null;

    private Image currentFrame = null;
    private int framesReceived = 0;
    #endregion

    #region Events
    public event NewFrameEventHandler NewFrame;
    #endregion

    #region Properties
    public ICaptureConfigs Configs { get; set; }
    public int FramesReceived
    {
      get { return (framesReceived); }
    }
    #endregion

    #region Constructors
    public JpegStream()
    {
    }
    #endregion

    #region Public Methods
    public bool Start()
    {
      if (Configs != null && thread == null)
      {
        framesReceived = 0;

        stopEvent.Reset();

        thread = new Thread(new ThreadStart(WorkerThread));
        thread.IsBackground = true;
        thread.Start();

        return (true);
      }

      return (false);
    }

    public void Stop()
    {
      if (thread != null)
      {
        if (!thread.Join(0))
        {
          stopEvent.Set();
          thread.Join();
        }

        thread = null;
      }
    }

    public Image Take()
    {
      lock (objLock)
      {
        return (new Bitmap(currentFrame));
      }
    }
    #endregion

    #region Private Methods
    private void WorkerThread()
    {
      byte[] buffer = new byte[bufSize];

      HttpWebRequest request = null;
      WebResponse response = null;
      Stream stream = null;
      Random rnd = new Random((int) DateTime.Now.Ticks);
      DateTime start;

      while (true)
      {
        int  read, total = 0;

        try
        {
          start = DateTime.Now;

          request = (HttpWebRequest)WebRequest.Create(
            Configs.Source + ((Configs.Source.IndexOf('?') == -1) ? '?' : '&') + "fake=" + rnd.Next().ToString());

          if (!string.IsNullOrEmpty(Configs.Username) && Configs.Password != null)
            request.Credentials = new NetworkCredential(Configs.Username, Configs.Password);

          response = request.GetResponse();

          stream = response.GetResponseStream();
          while (!stopEvent.WaitOne(0, true))
          {
            if (total > bufSize - readSize)
            {
              total = 0;
            }

            if ((read = stream.Read(buffer, total, readSize)) == 0)
              break;

            total += read;
          }

          if (!stopEvent.WaitOne(0, true))
          {
            ++framesReceived;

            lock (objLock)
            {
              currentFrame = Bitmap.FromStream(new MemoryStream(buffer, 0, total));
            }

            if (NewFrame != null)
            {
              NewFrame(this, new NewFrameEventArgs(currentFrame as Bitmap));
            }
          }

          if (Configs.FrameInterval > 0)
          {
            int msec = Configs.FrameInterval - (int)DateTime.Now.Subtract(start).TotalMilliseconds;

            while ((msec > 0) && (stopEvent.WaitOne(0, true) == false))
            {
              Thread.Sleep((msec < 100) ? msec : 100);
              msec -= 100;
            }
          }
        }
        catch (WebException ex)
        {
          Debug.Logging(ex.Message, "JpegStream");
          Thread.Sleep(250);
        }
        catch (Exception ex)
        {
          Debug.Logging(ex.Message, "JpegStream");
        }
        finally
        {
          if (request != null)
          {
            request.Abort();
            request = null;
          }

          if (stream != null)
          {
            stream.Close();
            stream = null;
          }

          if (response != null)
          {
            response.Close();
            response = null;
          }
        }

        if (stopEvent.WaitOne(0, true))
          break;
      }
    }
    #endregion

    #region Implementation of IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose managed resources
        stopEvent.Dispose();
      }
      // free native resources
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    #endregion
  }
}
