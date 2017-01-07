using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ghosen
{
	public class Message
	{
		private uint _arbId;

		public uint ArbId
		{
			get { return _arbId; }
			set { _arbId = value; }
		}

		private byte[] _dataBytes;

		public byte[] RawData
		{
			get { return _dataBytes; }
			set { _dataBytes = value; }
		}


		public static Message Parse(string messageString)
		{
			var ret = new Message();

			// Parse id first
			var arbIdMatcher = Regex.Match(messageString, @" ([A-Fa-f0-9]{3})#");

			// We could extract the arb id
			if (arbIdMatcher.Groups.Count == 2)
			{
				// extract the arb id
				var arbIdString = arbIdMatcher.Groups[1].Value;
				// parse the arb id
				var candidateArbId = uint.Parse(arbIdString, System.Globalization.NumberStyles.HexNumber);
				ret.ArbId = candidateArbId;
			}


			// Now handle data (only 8 byte packets at the moment)
			var rawDataMatcher = Regex.Match(messageString, @"#([A-Fa-f0-9]*)");

			// We could extract the raw data
			if (rawDataMatcher.Groups.Count == 2)
			{
				// extract the raw data
				var rawDataString = rawDataMatcher.Groups[1].Value;
				// parse the raw data
				var candidateRawData = Utils.StringToByteArrayFastest(rawDataString);
				ret.RawData = candidateRawData;
			}

			return ret;
		}

		public override string ToString()
		{
			return $@"{String.Format("{0:X3}", ArbId)}#{Utils.ByteArrayToHexViaLookup32(RawData)}";
		}
	}
}
