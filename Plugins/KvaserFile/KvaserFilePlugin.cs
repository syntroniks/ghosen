using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghosen.CAN;

namespace ghosen.Plugins
{
    public class KvaserFilePlugin : ghosen.IPluginV1
    {
        public string Name => "KvaserFilePlugin";

        public IEnumerable<string> SupportedFormats => new string[] { "Kvaser file dump" };

        public CAN.Message ParseLine(string line)
        {
            return KvaserLine.Parse(line).Message;
        }

        public IEnumerable<Message> ParseLines(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var tmp = KvaserLine.Parse(line).Message;
                if (tmp.ArbId == 0x7E0 ||
                    tmp.ArbId == 0x7E8)
                {
                    yield return tmp;
                }
            }
            yield break;
        }
    }
}
