using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229.Messages
{
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
}
