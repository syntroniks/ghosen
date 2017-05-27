using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.Plugins
{
    internal class CandumpParser
	{
		public static async Task<List<CandumpLine>> ParseStream(Stream s)
		{
			var ret = new List<CandumpLine>();

			var streamLength = s.Length;
			using (StreamReader sr = new StreamReader(s))
			{
				while (!sr.EndOfStream)
				{
					var progress = ((double)sr.BaseStream.Position / (double)sr.BaseStream.Length) * 100.0d;
					var currentLineStr = await sr.ReadLineAsync();
					var currentLine = CandumpLine.Parse(currentLineStr);
					Debug.WriteLine(currentLine);
					ret.Add(currentLine);
				}
			}

			return ret;
		}
		public static List<CandumpLine> ParseLines(string[] lines, CandumpParserArbIdFilter filter = null)
		{
			var ret = new List<CandumpLine>();
			var pastProgress = 0;
			for (int i = 0; i < lines.Length; i++)
			{
				var curProgress = ((i*100) / lines.Length);
				if (curProgress != pastProgress)
				{
					pastProgress = curProgress;
					//Debug.WriteLine(curProgress);
				}

				var currentLineStr = lines[i];
				var currentLine = CandumpLine.Parse(currentLineStr);
				if (filter != null)
				{
					if (filter.ShouldAcceptLine(currentLine))
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

			return ret;
		}
	}
}
