// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  public class EntryArgs
  {
    #region Public Properties
    public IGate Gate { get; set; }
    public IUniqueId UniqueId { get; set; }
    public IUserData UserData { get; set; }
    public ImagePair Images { get; set; }
    #endregion

    #region Public Constructors
    public EntryArgs(IUniqueId uniqueId, IUserData userData, ImagePair images)
    {
      this.UniqueId = uniqueId;
      this.UserData = userData;
      this.Images = images;
    }
    #endregion
  }
}
