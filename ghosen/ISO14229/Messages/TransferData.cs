using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229.Messages
{
    public class TransferData : ISO14229Message
    {
        public enum CompressionMethod
        {
            None
        }
        public enum EncryptionMethod
        {
            None
        }

        // request and response parameters
        public byte BlockSequenceCounter { get; set; }

        public byte[] DataRecord { get; set; }

        public TransferData(ISO_TP.Message message)
            : base(message)
        {
            //BlockSequenceCounter = message.Payload[1];
            DataRecord = message.Payload.Skip(1).ToArray();
            if (base.MessageType == ServiceMessageType.Request)
            {
            }
            else
            {
            }

        }

        public override string ToString()
        {
            return $"{Enum.GetName(typeof(ServiceType), Service)}{MessageType}: : {BlockSequenceCounter} [{Utils.ByteArrayToHexViaLookup32(DataRecord)}]";
        }
    }
}
