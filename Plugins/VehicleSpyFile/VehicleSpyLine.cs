using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ghosen.Plugins
{
	public class VehicleSpyLine
	{
		public DateTime Time { get; set; }

		public string Interface { get; set; }

		public CAN.Message Message { get; set; }

        public VehicleSpyLine()
        {
            Interface = "";
            Message = new CAN.Message();
        }

        private static readonly Regex timeRegex = new Regex(@"([0-9.]+)", RegexOptions.Compiled);
        private static readonly Regex idRegex = new Regex(@" ([A-Fa-f0-9]{3}) ", RegexOptions.Compiled);
        private static readonly Regex rawDataRegex = new Regex(@"( ([A-Fa-f0-9]{2} )+)", RegexOptions.Compiled);

        public static VehicleSpyLine Parse(string line)
		{
			var ret = new VehicleSpyLine();

			// Parse time first
			var timeMatch = timeRegex.Match(line);

			// We could extract the time
			if (timeMatch.Groups.Count == 2)
			{
				// extract the time
				var timeString = timeMatch.Groups[1].Value;

                // This is pretty fragile :<
				ret.Time = new DateTime().AddSeconds(double.Parse(timeString));
			}

            /*/ No support at the moment
			// Now get the interface name assuming the interface is of the form " [v]<can><0-9> " (note the spaces)
			var interfaceMatch = Regex.Match(line, @" (v?can[0-9]) ");
			if (interfaceMatch.Groups.Count == 2)
			{
				ret.Interface = interfaceMatch.Groups[1].Value;
			}
            //*/

            // Parse id first
            var arbIdMatcher = idRegex.Match(line);

            // We could extract the arb id
            if (arbIdMatcher.Groups.Count == 2)
            {
                // extract the arb id
                var arbIdString = arbIdMatcher.Groups[1].Value;
                // parse the arb id
                var candidateArbId = uint.Parse(arbIdString, System.Globalization.NumberStyles.HexNumber);
                ret.Message.ArbId = candidateArbId;
            }


            // Now handle data (only 8 byte packets at the moment)
            var rawDataMatcher = rawDataRegex.Match(line);

            // We could extract the raw data
            if (rawDataMatcher.Groups.Count >= 2)
            {
                // extract the raw data
                var rawDataString = rawDataMatcher.Groups[1].Value.Replace(" ", "");
                // parse the raw data
                var candidateRawData = Utils.StringToByteArrayFastest(rawDataString);
                ret.Message.RawData = candidateRawData;
            }
            else
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
