using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229.Messages
{
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
}
