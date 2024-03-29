﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.Plugins
{
  public class SavvyPlugin : ghosen.IPluginV1
  {
    /// <summary>
    /// This plugin is for parsing log files from the SavvyCAN Application
    /// https://www.savvycan.com/
    /// </summary>
    public override string Name => "SavvyFilePlugin";

    public override IEnumerable<string> SupportedFormats => new string[] { "Savvy CSV file" };

    public override CAN.Message ParseLine(string line)
    {
      return SavvyLine.Parse(line).Message;
    }

    public override IEnumerable<CAN.Message> ParseLines(IEnumerable<string> lines, IPluginLineFilterV1 filter = null)
    {
      foreach (var line in lines)
      {
        var parsedLine = SavvyLine.Parse(line);
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
