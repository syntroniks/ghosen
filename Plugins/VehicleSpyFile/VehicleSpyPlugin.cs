using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.Plugins
{
    public class VehicleSpyPlugin : ghosen.IPluginV1
	{
		public string Name => "VehicleSpyFilePlugin";

		public IEnumerable<string> SupportedFormats => new string[] { "Vehicle Spy dump" };

		public CAN.Message ParseLine(string line)
		{
			return VehicleSpyLine.Parse(line).Message;
		}

		public IEnumerable<CAN.Message> ParseLines(IEnumerable<string> lines, IPluginLineFilterV1 filter = null)
		{
			foreach (var line in lines)
			{
				var parsedLine = VehicleSpyLine.Parse(line);
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
