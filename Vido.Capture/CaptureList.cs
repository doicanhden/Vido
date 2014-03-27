
namespace Vido.Capture
{
  using System;
  using System.Collections.Generic;
  using Vido.Capture.Interfaces;
  public class CaptureList : ICaptureList, IDisposable
  {
    #region Data Members
    private readonly ICaptureFactory captureFactory;
    private readonly IList<ICapture> captures = new List<ICapture>();
    #endregion

    #region Public Constructors
    public CaptureList(ICaptureFactory captureFactory)
    {
      if (captureFactory == null)
      {
        throw new ArgumentNullException("captureFactory");
      }

      this.captureFactory = captureFactory;
    }
    #endregion

    #region Implementation of ICaptureList
    public ICollection<ICapture> Captures
    {
      get { return (captures); }
    }

    public ICapture Create(ICaptureConfigs configs)
    {
      var capture = captureFactory.Create(configs);
      if (capture != null)
      {
        captures.Add(capture);
      }

      return (capture);
    }
    #endregion

    #region Implementation of IDisposable
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // dispose managed resources
        foreach (var capture in Captures)
        {
          capture.Stop();
          capture.Dispose();
        }
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
