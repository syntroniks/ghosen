using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ghosen.Plugins
{
  public class SavvyLine
  {
    public DateTime Time { get; set; }

    public string Interface { get; set; }

    public CAN.Message Message { get; set; }

    public SavvyLine()
    {
      Interface = "";
      Message = new CAN.Message();
    }

    private static readonly Regex timeRegex = new Regex(@"([0-9.]+)", RegexOptions.Compiled);
    private static readonly Regex idRegex = new Regex(@" ([A-Fa-f0-9]{3}) ", RegexOptions.Compiled);
    private static readonly Regex rawDataRegex = new Regex(@"(([A-Fa-f0-9]{2})+)", RegexOptions.Compiled);

    public static SavvyLine Parse(string line)
    {
      var ret = new SavvyLine();
      const int dataFrameIndex = 6;
      const int fieldLen = 0xE;
      const int arbIdFrameIndex = 1;
      const int dataFrameCount = 5;

      //// Example Data Frame "7431355,000007E8,false,Rx,0,8,29,D8,59,81,8F,97,7B,D7,"
      // Parse time first
      var fields = line.Split(',');

      //Expect 14 Fields
      if (fields.Length < fieldLen) // probably not the line we want
      {
        return ret;
      }

      //// Check to see if we are on the header
      //// "Time Stamp,ID,Extended,Dir,Bus,LEN,D1,D2,D3,D4,D5,D6,D7,D8"
      if (String.Compare(fields[0], "Time Stamp") == 0)
      {
        return ret;
      }

      var timeMatch = timeRegex.Match(fields[0]);

      // We could extract the time
      if (timeMatch.Groups.Count == 2)
      {
        // extract the time
        var timeString = timeMatch.Groups[1].Value;

        // This is pretty fragile :<
        // Savvy Can Frames come in as Microseconds convert the to miliseconds
        ret.Time = new DateTime().AddMilliseconds(double.Parse(timeString) / 1000);
      }

      var arbIdString = fields[arbIdFrameIndex];

      // parse the arb id
      uint candidateArbId;
      bool arbIdMatch = uint.TryParse(arbIdString, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out candidateArbId);

      // We could extract the arb id
      if (arbIdMatch)
      {
        ret.Message.ArbId = candidateArbId;
      }

      var dataFrameString = fields[dataFrameCount];
      int candidateFrameLen;
      bool dataFrameLenMatch = int.TryParse(dataFrameString, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out candidateFrameLen);
      if (!(dataFrameLenMatch))
      {
        candidateFrameLen = 8;
      }

      // Now handle data
      StringBuilder sb = new StringBuilder();
      for (int i = dataFrameIndex; i < dataFrameIndex + candidateFrameLen; i++)
      {
        sb.Append(fields[i]);
      }

      // extract the raw data
      var rawDataString = sb.ToString();
      // parse the raw data
      // TODO: Better error handling. This fails on metadata lines
      try
      {
        var candidateRawData = Utils.StringToByteArray(rawDataString);
        ret.Message.RawData = candidateRawData;
      }
      catch (Exception)
      {
      }

      return ret;
    }

    private static DateTime DateTimeFromSpecialUnixTime(string unixTime)
    {
      var split = unixTime.Split('.');
      var seconds = int.Parse(split[0]);
      var microseconds = int.Parse(split[1]);
      const double SecondsPerMicrosecond = 0.000001d;
      const double TicksPerMicrosecond = TimeSpan.TicksPerSecond * SecondsPerMicrosecond;

      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

      return epoch.AddSeconds(seconds).AddTicks((long)(TicksPerMicrosecond * microseconds));
    }

    public override string ToString()
    {
      return $@"{Time.ToLocalTime()} {Interface} {Message}";
    }
  }
}
