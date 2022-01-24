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

      // Parse time first
      // TODO: maybe directly parse datetime
      var fields = line.Split(',');

      if (fields.Length < 0xE) // probably not the line we want
      {
        // should we return null?
        return ret;
      }

      //// We aren't using C#7's fancy out variable declaration
      //// because it isn't compatible with edit and continue.
      //// This is something that can be added as this plugin nears completion since it saves lines of code.
      //DateTime parsed;
      //var timeMatch = DateTime.TryParse(fields[0], out parsed);

      //// We could extract the time
      //if (timeMatch)
      //{
      //	ret.Time = parsed;

      //}
      var timeMatch = timeRegex.Match(fields[0]);

      // We could extract the time
      if (timeMatch.Groups.Count == 2)
      {
        // extract the time
        var timeString = timeMatch.Groups[1].Value;

        // This is pretty fragile :<
        ret.Time = new DateTime().AddMilliseconds(double.Parse(timeString)/1000);
      }

      /*/ No support at the moment
			// Now get the interface name assuming the interface is of the form " [v]<can><0-9> " (note the spaces)
			var interfaceMatch = Regex.Match(line, @" (v?can[0-9]) ");
			if (interfaceMatch.Groups.Count == 2)
			{
				ret.Interface = interfaceMatch.Groups[1].Value;
			}
            //*/

      // extract the arb id
      var arbIdString = fields[1];

      // parse the arb id
      uint candidateArbId;
      bool arbIdMatch = uint.TryParse(arbIdString, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out candidateArbId);

      // We could extract the arb id
      if (arbIdMatch)
      {
        ret.Message.ArbId = candidateArbId;
      }


      // Now handle data (only 8 byte packets at the moment)
      StringBuilder sb = new StringBuilder();
      for (int i = 6; i < 6 + 8; i++)
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
