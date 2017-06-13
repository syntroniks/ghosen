using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO_TP
{
	public class ISO_TP_Session
	{
		public static IEnumerable<ISO_TP.Message> ProcessFrames(IEnumerable<ghosen.CAN.Message> messages)
		{
			bool awaitingConsecutive = false;
			int awaitingLength = 0;
			List<byte> rawDataCollector = new List<byte>();
			List<byte[]> finishedMessages = new List<byte[]>();
			List<ISO_TP.Message> realMessages = new List<ISO_TP.Message>();

			foreach (var message in messages)
			{
				var parsedFrame = Framing.FrameParser.Parse(message.RawData);
                if (parsedFrame == null)
                {
                    // This is an error condition -- let's figure out how to handle this later...
                    // Maybe we should drop it
                    /*
                    yield return new ISO_TP.Message()
                    {
                        ArbId = new ArbitrationId(message.ArbId),
                        Payload = message.RawData,
                        Complete = true,
                        MultiFrame = false
                    };
                    */
                    continue;
                }
				switch (parsedFrame.FrameType)
				{
					case Framing.FrameType.Single:
						var castFrame3 = (Framing.SingleFrame)parsedFrame;
						finishedMessages.Add(castFrame3.RawData);
                        yield return new ISO_TP.Message()
                        {
							ArbId = new ArbitrationId(message.ArbId),
							Payload = castFrame3.RawData,
							PayloadSize = castFrame3.Length,
                            Complete = true,
                            MultiFrame = false
						};
						break;
					case Framing.FrameType.First:
						awaitingConsecutive = true;
						// grab first data bytes
						var castFrame = (Framing.FirstFrame)parsedFrame;
						awaitingLength = castFrame.Length;
						rawDataCollector = new List<byte>(awaitingLength);
						rawDataCollector.AddRange(castFrame.RawData);
						break;
					case Framing.FrameType.Consecutive:
						if (awaitingConsecutive)
						{
							// only clear awaiting if we have received enough bytes
							var castFrame1 = (Framing.ConsecutiveFrame)parsedFrame;

							// Maybe we shouldn't add all of the raw data -- the last 
							// fragment may be a partial fragment (there may be extra bytes at the end)
							rawDataCollector.AddRange(castFrame1.RawData);
							if (rawDataCollector.Count >= awaitingLength)
							{
								awaitingLength = 0;
								awaitingConsecutive = false;
								finishedMessages.Add(rawDataCollector.ToArray());
                                yield return new ISO_TP.Message()
                                {
                                    ArbId = new ArbitrationId(message.ArbId),
                                    Payload = rawDataCollector.ToArray(),
                                    PayloadSize = awaitingLength,
                                    Complete = true,
                                    MultiFrame = true
                                };
							}
						}
						else
						{
							// error
						}
						break;
					case Framing.FrameType.Flow:
						// we don't care about flow control for this part of the process
						break;
					default:
						break;
				}
			}
            yield break;
		}
	}

	public class Framing
	{
		// single, first, consecutive, flow
		public enum FrameType
		{
			Single = 0,			// The single frame transferred contains the complete payload of up to 7 bytes (normal addressing) or 6 bytes (extended addressing)
			First = 1,			// The first frame of a longer multi-frame message packet, 
								// used when more than 6/7 bytes of data segmented must be communicated. 
								// The first frame contains the length of the full packet, along with the initial data.
			Consecutive = 2,    // A frame containing subsequent data for a multi-frame packet
			Flow = 3            // The response from the receiver, acknowledging a First-frame segment. 
								// It lays down the parameters for the transmission of further consecutive frames.
		}
		static FrameType DetermineFrameType(byte b)
		{
			return (FrameType)(b >> 4); // high nibble
		}

		public class FrameParser
		{
			public static Frame Parse(byte[] message)
			{
				Frame ret = null;
				switch (DetermineFrameType(message[0]))
				{
					case FrameType.Single:
						ret = new SingleFrame(message);
						break;
					case FrameType.First:
						ret = new FirstFrame(message);
						break;
					case FrameType.Consecutive:
						ret = new ConsecutiveFrame(message);
						break;
					case FrameType.Flow:
						ret = new FlowControlFrame(message);
						break;
					default:
						break;
				}
				return ret;
			}
		}

		public class Frame
		{
			public FrameType FrameType { get; set; }
			public byte[] RawData { get; set; }

			public Frame(byte[] message)
			{
				FrameType = DetermineFrameType(message[0]);
			}
		}

		public class SingleFrame : Frame
		{
			public byte Length { get; set; }

			public SingleFrame(byte[] message)
				: base(message)
			{
				// first nibble is frame type

				Length = (byte)(message[0] & 0x0F);

				// skip the first two (frame type and length)
				RawData = message.Skip(1).Take(Length).ToArray();
			}

		}

		public class FirstFrame : Frame
		{
			public ushort Length { get; set; }

			public FirstFrame(byte[] message)
				: base(message)
			{
				// first nibble is frame type

				// assuming msb first
				Length = (ushort)((message[0] & 0x0F) << 8 | message[1]);

				// get raw data guaranteed by spec to be > 6/7 bytes
				// skip the first two (frame type and length)
				RawData = message.Skip(2).ToArray();
			}

		}

		public class ConsecutiveFrame : Frame
		{
			public byte Index { get; set; } // 0x00 - 0x0F

			public ConsecutiveFrame(byte[] message)
				: base(message)
			{
				// first nibble is frame type
				Index = GetIndex(message[0]);

				// skip the first byte (frame type and index)
				RawData = message.Skip(1).ToArray();
			}

			private byte GetIndex(byte b)
			{
				return (byte)(b & 0x0F);
			}
		}

		public class FlowControlFrame : Frame
		{
			public byte BlockSize { get; set; } // if block size == 0, no flow control
			public byte SeparationTime { get; set; } // not currently implemented
			public FlowControlAllowTransfer AllowTransfer { get; set; }

			public enum FlowControlAllowTransfer
			{
				ClearToSend = 0,
				Wait = 1,
				Overflow = 2
			}

			public FlowControlFrame(byte[] message)
				: base(message)
			{
				// first nibble is frame type
				AllowTransfer = GetAllowTransfer(message[0]);
				BlockSize = message[1];
				SeparationTime = message[2];
			}

			private FlowControlAllowTransfer GetAllowTransfer(byte b)
			{
				return (FlowControlAllowTransfer)(b & 0x0F);
			}
		}
	}
}
