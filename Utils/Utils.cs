using System;
using System.Collections.Generic;
using System.Linq;

namespace ghosen
{
  public class Utils
  {
    public static byte[] StringToByteArray(string hex)
    {
      return Enumerable.Range(0, hex.Length)
               .Where(x => x % 2 == 0)
               .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
               .ToArray();
    }

    public static byte[] StringToByteArrayFastest(string hex)
    {
      if (hex.Length % 2 == 1)
        throw new Exception("The binary key cannot have an odd number of digits");

      byte[] arr = new byte[hex.Length >> 1];

      for (int i = 0; i < (hex.Length >> 1); ++i)
      {
        arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
      }

      return arr;
    }

    public static int GetHexVal(char hex)
    {
      int val = hex;
      //For uppercase A-F letters:
      return val - (val < 58 ? 48 : 55);
      //For lowercase a-f letters:
      //return val - (val < 58 ? 48 : 87);
      //Or the two combined, but a bit slower:
      //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
    }

    private static readonly uint[] _lookup32 = CreateLookup32();

    private static uint[] CreateLookup32()
    {
      var result = new uint[256];
      for (int i = 0; i < 256; i++)
      {
        string s = i.ToString("X2");
        result[i] = s[0] + ((uint)s[1] << 16);
      }
      return result;
    }

    public static string ByteArrayToHexViaLookup32(IEnumerable<byte> _bytes)
    {
      var bytes = _bytes.ToArray();
      var lookup32 = _lookup32;
      var result = new char[bytes.Length * 2];
      for (int i = 0; i < bytes.Length; i++)
      {
        var val = lookup32[bytes[i]];
        result[2 * i] = (char)val;
        result[2 * i + 1] = (char)(val >> 16);
      }
      return new string(result);
    }
  }
}
