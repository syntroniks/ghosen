using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghosen.ISO_TP;

namespace ghosen.ISO14229
{
	public class DiagnosticSessionControl : ISO14229Message
	{
		public enum DiagnosticSessionType
		{
			DefaultSession = 0x01,
			ProgrammingSession = 0x02,
			ExtendedDiagnosticSession = 0x03,
			SafetySystemDiagnosticSession = 0x04
		}

		public DiagnosticSessionType SessionType { get; set; }

		public DiagnosticSessionControl(ISO_TP.Message message)
			: base(message)
		{
			SessionType = (DiagnosticSessionType)message.Payload[1];
			if (base.MessageType == ServiceMessageType.Request)
			{
			}
			else
			{

			}
		}
	}
	public class SecurityAccessControl : ISO14229Message
	{
		public enum SecurityAccessControlType
		{
			RequestSeed,
			SendKey,
		}

		private SecurityAccessControlType GetSACType(byte b)
		{
			if (b == 0x01 ||
				b == 0x03 ||
				b == 0x05)
				return SecurityAccessControlType.RequestSeed;

			if (b == 0x02 ||
				b == 0x04 ||
				b == 0x06)
				return SecurityAccessControlType.SendKey;

			// default, we should not hit this. Consider throwing an exception
			return SecurityAccessControlType.RequestSeed;
		}

		public SecurityAccessControlType SubFunction { get; set; }

		public byte[] SecurityAccessDataRecord { get; set; }

		public SecurityAccessControl(ISO_TP.Message message)
			: base(message)
		{
			SubFunction = GetSACType(message.Payload[1]);
			if (base.MessageType == ServiceMessageType.Request)
			{
				SecurityAccessDataRecord = message.Payload.Skip(2).ToArray();
			}
			else
			{
				// if it is a response, this is our seed
				SecurityAccessDataRecord = message.Payload.Skip(2).ToArray();
			}
		}

		public override string ToString()
		{
			return $"{base.ToString()} : {SubFunction} [{Utils.ByteArrayToHexViaLookup32(SecurityAccessDataRecord)}]";
		}
	}
	public class RoutineControl : ISO14229Message
	{
		public enum RoutineControlType
		{
			ISOSAEReserved,
			StartRoutine,
			StopRoutine,
			RequestRoutineResults,
		}

		public RoutineControlType SubFunction { get; set; }

		public UInt16 RoutineIdentifier { get; set; }

		public byte[] RoutineControlOptionRecord { get; set; }

		public RoutineControl(ISO_TP.Message message)
			: base(message)
		{
			SubFunction = (RoutineControlType)message.Payload[1];
			RoutineIdentifier = BitConverter.ToUInt16(message.Payload, 2);
			RoutineControlOptionRecord = message.Payload.Skip(4).ToArray();
			if (base.MessageType == ServiceMessageType.Request)
			{
			}
			else
			{
			}
		}

		public override string ToString()
		{
			return $"{base.ToString()} : {SubFunction} {RoutineIdentifier} [{Utils.ByteArrayToHexViaLookup32(RoutineControlOptionRecord)}]";
		}
	}
	public class RequestDownload : ISO14229Message
	{
		public enum CompressionMethod
		{
			None
		}
		public enum EncryptionMethod
		{
			None
		}

		// request  parameters
		public CompressionMethod Compression { get; set; }
		public EncryptionMethod Encryption { get; set; }

		public int MemorySize { get; set; }
		public uint MemoryAddress { get; set; }

		// response parameters
		public int MaxNumberOfBlockLengthLength { get; set; }
		public int MaxNumberOfBlockLength { get; set; }

		public RequestDownload(ISO_TP.Message message)
			: base(message)
		{
			if (base.MessageType == ServiceMessageType.Request)
			{
				// High nibble
				Compression = (CompressionMethod)((message.Payload[1] >> 4) & 0x0F);
				// low nibble
				Encryption = (EncryptionMethod)((message.Payload[1] >> 0) & 0x0F);

				// High nibble
				var memorySizeLength = (int)((message.Payload[2] >> 4) & 0x0F);
				// low nibble
				var memoryAddressLength = (int)((message.Payload[2] >> 0) & 0x0F);

				// Memory address MSB first
				for (int i = 0; i < memoryAddressLength; i++)
				{
					// Larger shifting because we process MSB first
					var shiftAmount = 8 * (memoryAddressLength - i - 1);

					// build memory address one byte at a time
					MemoryAddress += (uint)message.Payload[3 + i] << shiftAmount;
				}

				// Same thing for memory size
				for (int i = 0; i < memorySizeLength; i++)
				{
					// Larger shifting because we process MSB first
					var shiftAmount = 8 * (memorySizeLength - i - 1);

					// build memory size one byte at a time
					MemorySize += message.Payload[memoryAddressLength + 3 + i] << shiftAmount;
				}
			}
			else
			{
				MaxNumberOfBlockLengthLength = (int)((message.Payload[1] >> 4) & 0x0F);
				for (int i = 0; i < MaxNumberOfBlockLengthLength; i++)
				{
					// Larger shifting because we process MSB first
					var shiftAmount = 8 * (MaxNumberOfBlockLengthLength - i - 1);

					// build memory size one byte at a time
					MaxNumberOfBlockLength += message.Payload[2 + i] << shiftAmount;
				}
			}
		}

		public override string ToString()
		{
			return $"{base.ToString()} : 0x{MemoryAddress:X} : {MemorySize}";
		}
	}
	public class RequestUpload : RequestDownload
	{
		public RequestUpload(ISO_TP.Message message)
			: base(message)
		{
		}
	}
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

		internal static ISO14229Message Create(Message message)
		{
			// try to get service id
			// also get request / response
			var isRequest = message.Payload[0] < 0x40;
			var offset = (isRequest) ? 0 : 0x40;
			var service = (ServiceType)(message.Payload[0] - offset);

			switch (service)
			{
				case ServiceType.DiagnosticSessionControl:
					return new DiagnosticSessionControl(message);
					break;
				case ServiceType.ECUReset:
					break;
				case ServiceType.SecurityAccess:
					return new SecurityAccessControl(message);
					break;
				case ServiceType.CommunicationControl:
					break;
				case ServiceType.TesterPresent:
					break;
				case ServiceType.AccessTimingParameter:
					break;
				case ServiceType.SecuredDataTransmission:
					break;
				case ServiceType.ControlDTCSetting:
					break;
				case ServiceType.ResponseOnEvent:
					break;
				case ServiceType.LinkControl:
					break;
				case ServiceType.ReadDataByIdentifier:
					break;
				case ServiceType.ReadMemoryByAddress:
					break;
				case ServiceType.ReadScalingDataByIdentifier:
					break;
				case ServiceType.ReadDataByPeriodicIdentifier:
					break;
				case ServiceType.DynamicallyDefineDataIdentifier:
					break;
				case ServiceType.WriteDataByIdentifier:
					break;
				case ServiceType.WriteMemoryByAddress:
					break;
				case ServiceType.RoutineControl:
					return new RoutineControl(message);
					break;
				case ServiceType.RequestDownload:
					return new RequestDownload(message);
					break;
				case ServiceType.RequestUpload:
					return new RequestUpload(message);
					break;
				case ServiceType.TransferData:
					break;
				case ServiceType.RequestTransferExit:
					break;
				case ServiceType.RequestFileTransfer:
					break;
				default:
					break;
			}
			return new ISO14229Message(message);
		}
	}

	public class Messages
	{
		public static List<ISO14229Message> ProcessMessages(List<ISO_TP.Message> messages)
		{
			List<ISO14229Message> ret = new List<ISO14229Message>();
			for (int i = 0; i < messages.Count; i++)
			{
				// .Create tries to make a specific child class if possible.
				// Otherwise, it just makes the base class
				ret.Add(ISO14229.ISO14229Message.Create(messages[i]));
			}
			return ret;
		}
	}
}
