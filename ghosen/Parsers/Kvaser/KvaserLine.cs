using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ghosen.Parsers.Kvaser
{
	public class KvaserLine
    {
		public DateTime Time { get; set; }

		public string Interface { get; set; }

		public CAN.Message Message { get; set; }

        public KvaserLine()
        {
            Interface = "";
            Message = new CAN.Message();
        }

		public static KvaserLine Parse(string line)
		{
			var ret = new KvaserLine();

			// Parse time first
			var timeMatch = Regex.Match(line, @"([0-9.]+)");

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

			ret.Message = KvaserParser.ParseKvaserMessage(line);

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
