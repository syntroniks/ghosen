using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.Plugins
{
    /// <summary>
    /// This class implements an optional <see cref="CandumpParser"/> filter.
    /// This filter passes messages which have an arbitration ID matching one the filter is configured to accept.
    /// All other messages are dropped
    /// </summary>
	public class KvaserParserArbIdFilter
	{
		public IList<uint> ArbIds { get; private set; }

        /// <summary>
        /// Construct a new filter with the given arbitration ids
        /// </summary>
        /// <param name="arbIds">a collection of arbitration ids this filter will accept</param>
		public KvaserParserArbIdFilter(IList<uint> arbIds)
		{
			ArbIds = arbIds;
		}

        /// <summary>
        /// This function calculates whether a given candump line should be accepted or not
        /// </summary>
        /// <param name="line">the candump line to test for acceptance</param>
        /// <returns>true if the given <see cref="KvaserLine"/> should be accepted, otherwise false</returns>
		public bool ShouldAcceptLine(KvaserLine line)
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
