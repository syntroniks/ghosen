using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229.Messages
{
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
}
