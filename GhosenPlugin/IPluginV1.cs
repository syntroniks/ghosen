using System;
using System.Collections.Generic;

namespace ghosen
{
  public abstract class IPluginV1
  {
    /// <summary>
    /// The name of this plugin
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Return a collection of supported formats. This is displayed to the user
    /// </summary>
    public abstract IEnumerable<String> SupportedFormats { get; }

    /// <summary>
    /// Parse a given input line into a can message.
    /// If a line could not be parsed, it is inclear what should be done
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public abstract CAN.Message ParseLine(string line);

    public abstract IEnumerable<CAN.Message> ParseLines(IEnumerable<string> lines, IPluginLineFilterV1 filter = null);

    public override string ToString() => Name;
  }

  public interface IPluginLineFilterV1
  {
    bool ShouldAcceptLine(CAN.Message msg);
  }
}
