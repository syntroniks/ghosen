using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ghosen.Plugins;

namespace ghosen.Tests
{
    [TestClass]
    public class KvaserTextFileTests
    {
        [TestMethod]
        public void ReturnsNullForHeader()
        {
            var header = "Chn Identifier Flg   DLC  D0...1...2...3...4...5...6..D7       Time     Dir";
            Assert.IsNull(KvaserTextLine.Parse(header));
        }

        [TestMethod]
        public void ReturnsNoDataForErrorFrame()
        {
            var errorFrame = "0    00000000            ErrorFrame                          29.095270 R";
            Assert.AreEqual(0, KvaserTextLine.Parse(errorFrame).Message.RawData.Length);
        }

        [TestMethod]
        public void ReturnsNullForLoggingMessage()
        {
            var loggingMessage = "Logging stopped.";
            Assert.IsNull(KvaserTextLine.Parse(loggingMessage));
        }

        [TestMethod]
        public void ParsesDataFrame()
        {
            var dataFrame = " 0    000007E8         8  03  59  02  FF  00  00  00  00     395.000970 R";
            var uut = KvaserTextLine.Parse(dataFrame);
            Assert.AreEqual((UInt32)0x7E8, uut.Message.ArbId);
            Assert.AreEqual(8, uut.Message.RawData.Length);
            Assert.IsTrue(new byte[] { 0x03, 0x59, 0x02, 0xFF, 0x00, 0x00, 0x00, 0x00 }.SequenceEqual(uut.Message.RawData));
            Assert.AreEqual(new DateTime().AddSeconds(395.000970), uut.Time);
        }
    }
}
