using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ghosen.Plugins
{
    public class KvaserTextParser
    {
        /*/ Unused currently
        public static async Task<List<KvaserLine>> ParseStream(Stream s)
        {
            var ret = new List<KvaserLine>();

            var streamLength = s.Length;
            using (StreamReader sr = new StreamReader(s))
            {
                while (!sr.EndOfStream)
                {
                    var progress = ((double)sr.BaseStream.Position / (double)sr.BaseStream.Length) * 100.0d;
                    var currentLineStr = await sr.ReadLineAsync();
                    var currentLine = KvaserLine.Parse(currentLineStr);
                    Debug.WriteLine(currentLine);
                    ret.Add(currentLine);
                }
            }

            return ret;
        }
        //*/

        public static List<KvaserTextLine> ParseLines(string[] lines, IPluginLineFilterV1 filter = null)
        {
            // first strip the header out

            var ret = new List<KvaserTextLine>();
            var pastProgress = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var curProgress = ((i * 100) / lines.Length);
                if (curProgress != pastProgress)
                {
                    pastProgress = curProgress;
                    //Debug.WriteLine(curProgress);
                }

                var currentLineStr = lines[i];
                var currentLine = KvaserTextLine.Parse(currentLineStr);
                if (currentLine.Message != null)
                {
                    if (filter != null)
                    {
                        if (filter.ShouldAcceptLine(currentLine.Message))
                        {
                            ret.Add(currentLine);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        // we have no filter, accept all
                        ret.Add(currentLine);
                    }
                }
            }

            return ret;
        }

    }
}
