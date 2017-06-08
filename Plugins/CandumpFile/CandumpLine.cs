using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ghosen.Plugins
{
    internal class CandumpLine
	{
		public DateTime Time { get; set; }

		public string Interface { get; set; }

		public CAN.Message Message { get; set; }

        public CandumpLine()
        {
            Interface = "";
            Message = new CAN.Message();
        }

		public static CandumpLine Parse(string line)
		{
			var ret = new CandumpLine();

			// Parse time first
			var timeMatch = Regex.Match(line, @"\(([0-9|.]*)\)");

			// We could extract the time
			if (timeMatch.Groups.Count == 2)
			{
				// extract the time
				var timeString = timeMatch.Groups[1].Value;
				ret.Time = DateTimeFromSpecialUnixTime(timeString);
			}

			// Now get the interface name assuming the interface is of the form " [v]<can><0-9> " (note the spaces)
			var interfaceMatch = Regex.Match(line, @" (v?can[0-9]) ");
			if (interfaceMatch.Groups.Count == 2)
			{
				ret.Interface = interfaceMatch.Groups[1].Value;
			}
            
			ret.Message = ParseCandumpMessage(line);

            return ret;
		}

        public static CAN.Message ParseCandumpMessage(string messageString)
        {
            var ret = new CAN.Message();

            // Parse id first
            var arbIdMatcher = Regex.Match(messageString, @" ([A-Fa-f0-9]{3})#");

            // We could extract the arb id
            if (arbIdMatcher.Groups.Count == 2)
            {
                // extract the arb id
                var arbIdString = arbIdMatcher.Groups[1].Value;
                // parse the arb id
                var candidateArbId = uint.Parse(arbIdString, System.Globalization.NumberStyles.HexNumber);
                ret.ArbId = candidateArbId;
            }
            else
            {
                return null;
            }


            // Now handle data (only 8 byte packets at the moment)
            var rawDataMatcher = Regex.Match(messageString, @"#([A-Fa-f0-9]*)");

            // We could extract the raw data
            if (rawDataMatcher.Groups.Count == 2)
            {
                // extract the raw data
                var rawDataString = rawDataMatcher.Groups[1].Value;
                // parse the raw data
                var candidateRawData = Utils.StringToByteArrayFastest(rawDataString);
                ret.RawData = candidateRawData;
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
