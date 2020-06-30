using System.Collections.Generic;

namespace ghosen.Plugins
{
  /// <summary>
  /// This class implements an optional <see cref="IPluginLineFilterV1"/> filter.
  /// This filter passes messages which have an arbitration ID matching one the filter is configured to accept.
  /// All other messages are dropped
  /// </summary>
  public class ArbIdFilter : IPluginLineFilterV1
  {
    public IList<uint> ArbIds { get; private set; }

    /// <summary>
    /// Construct a new filter with the given arbitration ids
    /// </summary>
    /// <param name="arbIds">a collection of arbitration ids this filter will accept</param>
    public ArbIdFilter(IList<uint> arbIds)
    {
      ArbIds = arbIds;
    }

    /// <summary>
    /// This function calculates whether a given candump line should be accepted or not
    /// </summary>
    /// <param name="line">the candump line to test for acceptance</param>
    /// <returns>true if the given <see cref="string"/> should be accepted, otherwise false</returns>
    public bool ShouldAcceptLine(string line)
    {
      return false;
    }

    public bool ShouldAcceptLine(CAN.Message message)
    {
      for (int i = 0; i < ArbIds.Count; i++)
      {
        if (message.ArbId == ArbIds[i])
          return true;
      }
      return false;
    }
  }
}
