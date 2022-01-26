using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ghosen.Plugins;

namespace ghosen.Tests
{
  [TestClass]
  public class SavvyTests
  {
    /// <summary>
    /// An empty line will leave all fields default initialized
    /// </summary>
    [TestMethod]
    public void SavvyEmptyLineTest()
    {
      // arrange
      var testLine = "";

      // act
      var result = ghosen.Plugins.SavvyLine.Parse(testLine);

      // assert
      Assert.AreEqual(default(DateTime), result.Time);
      Assert.AreEqual(string.Empty, result.Interface);
      Assert.AreEqual(null, result.Message);
    }

    /// <summary>
    /// Confirm a dataframe can be parsed
    /// </summary>
    [TestMethod]
    public void SavvyParsesDataFrame()
    {
      // arrange
      var testLine = "7431355,000007E8,false,Rx,0,8,29,D8,59,81,8F,97,7B,D7,";

      // act
      var result = ghosen.Plugins.SavvyLine.Parse(testLine);

      // assert
      Assert.AreEqual((UInt32)0x7E8, result.Message.ArbId);
      Assert.AreEqual(8, result.Message.RawData.Length);
      Assert.IsTrue(new byte[] { 0x29, 0xD8, 0x59, 0x81, 0x8F, 0x97, 0x7B, 0xD7 }.SequenceEqual(result.Message.RawData));
      Assert.AreEqual(new DateTime().AddMilliseconds(7431355/1000), result.Time);
    }
  }
}
