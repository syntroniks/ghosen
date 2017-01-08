using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229
{
	public class ISO14229Message
	{
		public ServiceType Service { get; set; }
		public ServiceMessageType MessageType { get; set; }
		public byte[] Data { get; set; }
		
		public ISO14229Message(ISO_TP.Message msg)
		{
			if (msg.Payload[0] > 0x3F)
			{
				// responses
				Service = (ServiceType)(msg.Payload[0] - 0x40);
				MessageType = ServiceMessageType.Response;
			}
			else
			{
				Service = (ServiceType)(msg.Payload[0]);
				MessageType = ServiceMessageType.Request;
			}

			Data = msg.Payload;
		}

		public override string ToString()
		{
			return $"{Enum.GetName(typeof(ServiceType), Service)}{MessageType}: {{{Utils.ByteArrayToHexViaLookup32(Data)}}}";
		}
	}

	public class Messages
	{
		public static List<ISO14229Message> ProcessMessages(List<ISO_TP.Message> messages)
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
