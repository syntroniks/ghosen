using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ghosen.Tests
{
    [TestClass]
    public class CanDumpTests
    {
        /// <summary>
        /// An empty line will leave all fields default initialized
        /// </summary>
        [TestMethod]
        public void CanDumpLineTest()
        {
            // arrange
            var testLine = "";

            // act
            var result = ghosen.Candump.CandumpLine.Parse(testLine);

            // assert
            Assert.AreEqual(default(string), result.Interface);
            Assert.AreEqual(new CAN.Message(), result.Message);
            Assert.AreEqual(default(DateTime), result.Time);
        }
    }
}
