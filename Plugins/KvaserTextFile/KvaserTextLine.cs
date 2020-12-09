using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ghosen.Plugins
{
    public class KvaserTextLine
    {
        public DateTime Time { get; set; }

        public string Interface { get; set; }

        public CAN.Message Message { get; set; }

        public KvaserTextLine()
        {
            Interface = "";
            Message = new CAN.Message();
        }

        private static readonly Regex timeRegex = new Regex(@"([0-9.]+)", RegexOptions.Compiled);
        private static readonly Regex idRegex = new Regex(@" ([A-Fa-f0-9]{8}) ", RegexOptions.Compiled);
        private static readonly Regex rawDataRegex = new Regex(@"( ([A-Fa-f0-9]{2} )+)", RegexOptions.Compiled);

        public static KvaserTextLine Parse(string line)
        {
            // 😢
            try
            {
                var ret = new KvaserTextLine();

                // split!
                var split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var numFields = split.Length;
                var channel = split[0];
                var arbIdString = split[1];
                var dlc = split[2];
                var timeString = split[numFields - 2];
                var data = split.Skip(3).Take(numFields - 5);

                // Parse time first
                if (double.TryParse(timeString, out var timeSeconds))
                {
                    ret.Time = new DateTime().AddSeconds(timeSeconds);
                }

                // parse the arb id
                var candidateArbId = uint.Parse(arbIdString, System.Globalization.NumberStyles.HexNumber);
                ret.Message.ArbId = candidateArbId;

                // parse the raw data
                var candidateRawData = Utils.StringToByteArrayFastest(String.Join("", data));
                ret.Message.RawData = candidateRawData;

                return ret;
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public override string ToString()
        {
            return $@"{Time.ToLocalTime()} {Interface} {Message}";
        }
    }
}
