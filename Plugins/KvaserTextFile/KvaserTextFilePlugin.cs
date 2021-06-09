using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghosen.CAN;

namespace ghosen.Plugins
{
    public class KvaserTextFilePlugin : ghosen.IPluginV1
    {
        public override string Name => "KvaserTextFilePlugin";

        public override IEnumerable<string> SupportedFormats => new string[] { "Kvaser file dump" };

        public override CAN.Message ParseLine(string line)
        {
            return KvaserTextLine.Parse(line).Message;
        }

        public override IEnumerable<CAN.Message> ParseLines(IEnumerable<string> lines, IPluginLineFilterV1 filter = null)
        {
            foreach (var line in lines)
            {
                var parsedLine = KvaserTextLine.Parse(line);
                // If we have a problem with the parsed line, just forget about it
                if (parsedLine == null)
                {
                    continue;
                }

                // If we've got a filter
                if (filter != null)
                {
                    // use it
                    if (filter.ShouldAcceptLine(parsedLine.Message))
                    {
                        var parsedMessage = parsedLine.Message;
                        yield return parsedMessage;
                    }
                    else
                    {
                        // This message didn't meet the filter
                        continue;
                    }
                }
                else
                {
                    // no filter, yield this element
                    yield return parsedLine.Message;
                }
            }
            yield break;
        }
    }
}
