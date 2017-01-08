using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229
{
	public class ISO14229Message
	{
		public Services Service { get; set; }
		public bool IsResponse { get; set; }

		public ISO14229Message(byte[] message)
		{
			if (message[0] > 0x3F)
			{
				// responses
				Service = (Services)(message[0] - 0x40);
				IsResponse = true;
			}
			else
			{
				Service = (Services)(message[0]);
				IsResponse = false;
			}
		}

		public override string ToString()
		{
			return $@"[{IsResponse} {Enum.GetName(typeof(Services), Service)}]";
		}
	}

	public class Messages
	{
		public static List<ISO14229Message> ProcessMessages(List<byte[]> messages)
		{
			List<ISO14229Message> ret = new List<ISO14229Message>();
			for (int i = 0; i < messages.Count; i++)
			{
				ret.Add(new ISO14229.ISO14229Message(messages[i]));
			}
			return ret;
		}
	}
}
