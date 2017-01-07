using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen
{
	public class CandumpParserArbIdFilter
	{
		public List<uint> ArbIds { get; private set; }

		public CandumpParserArbIdFilter(List<uint> arbIds)
		{
			ArbIds = arbIds;
		}

		public bool ShouldAcceptLine(CandumpLine line)
		{
			for (int i = 0; i < ArbIds.Count; i++)
			{
				if (line.Message.ArbId == ArbIds[i])
					return true;
			}

			return false;
		}
	}
}
