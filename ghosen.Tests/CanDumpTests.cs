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
        public void CanDumpEmptyLineTest()
        {
            throw new NotImplementedException("Tests broken due to plugin refactoring");
            /*
            // arrange
            var testLine = "";

            // act
            var result = ghosen.Plugins.CandumpFilePlugin.CandumpLine.Parse(testLine);

            // assert
            Assert.AreEqual(default(DateTime), result.Time);
            Assert.AreEqual(string.Empty, result.Interface);
            Assert.AreEqual(new CAN.Message(), result.Message);
            */
        }

        /// <summary>
        /// A partial line will leave unspecified fields default initialized
        /// </summary>
        [TestMethod]
        public void CanDumpPartialLineTest()
        {
            throw new NotImplementedException("Tests broken due to plugin refactoring");
            /*
            // arrange
            var testLine = "(1400000000.1000) vcan6 ";

            // act
            var result = ghosen.Candump.CandumpLine.Parse(testLine);

            // assert
            Assert.AreEqual(DateTime.Parse("5/13/2014 4:53:20 PM").Date, result.Time.Date);
            Assert.AreEqual("vcan6", result.Interface);
            Assert.AreEqual(new CAN.Message(), result.Message);
            */
        }
    }
}
