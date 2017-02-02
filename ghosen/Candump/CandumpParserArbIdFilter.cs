using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.Candump
{
    /// <summary>
    /// This class implements an optional <see cref="CandumpParser"/> filter.
    /// This filter passes messages which have an arbitration ID matching one the filter is configured to accept.
    /// All other messages are dropped
    /// </summary>
	public class CandumpParserArbIdFilter
	{
		public IList<uint> ArbIds { get; private set; }

        /// <summary>
        /// Construct a new filter with the given arbitration ids
        /// </summary>
        /// <param name="arbIds">a collection of arbitration ids this filter will accept</param>
		public CandumpParserArbIdFilter(IList<uint> arbIds)
		{
			ArbIds = arbIds;
		}

        /// <summary>
        /// This function calculates whether a given candump line should be accepted or not
        /// </summary>
        /// <param name="line">the candump line to test for acceptance</param>
        /// <returns>true if the given <see cref="CandumpLine"/> should be accepted, otherwise false</returns>
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
