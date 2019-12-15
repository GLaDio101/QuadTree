using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Project.Utils
{
  public static class StringExtensions
  {
    public static IEnumerable Utf8ToCodePoints(this string s)
    {
      var utf32bytes = Encoding.UTF32.GetBytes(s);
      var bytesPerCharInUtf32 = 4;
      Debug.Assert(utf32bytes.Length % bytesPerCharInUtf32 == 0);
      for (int i = 0; i < utf32bytes.Length; i += bytesPerCharInUtf32)
      {
        yield return BitConverter.ToInt32(utf32bytes, i);
      }
    }

//    public static string GetUTF32String(string s)
//    {
//      IEnumerable<string> x = s.Utf8ToCodePoints().Select(p => string.Format("U+{0:X4}", p));
//      var pointsAsString = string.Join(";", x.ToArray());
//      return pointsAsString;
//    }
  }
}