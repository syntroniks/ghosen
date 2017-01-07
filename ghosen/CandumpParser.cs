using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen
{
	public class CandumpParser
	{
		// open file
		// spin off worker
		// {
		// read file line
		// parse file line
		// return file line to thread safe queue
		// }
		// worker completed

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
	}
}
