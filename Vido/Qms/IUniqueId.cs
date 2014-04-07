// Copyright (C) 2014 Vido's R&D.  All rights reserved.

namespace Vido.Qms
{
  using System;
  using System.Text;

  public interface IUniqueId
  {
    byte[] UniqueId { get; set; }

    bool Printable { get; set; }
  }

  public static class IUniqueIdExtensions
  {
    public static string ToPrintable(this IUniqueId data)
    {
      if (data.Printable)
        return (Encoding.Unicode.GetString(data.UniqueId, 0, data.UniqueId.Length));

      return (Convert.ToBase64String(data.UniqueId));
    }
  }
}