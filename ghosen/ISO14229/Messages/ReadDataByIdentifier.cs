using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229.Messages
{
    public class ReadDataByIdentifier : ISO14229Message
    {
        public ushort Identifier { get; set; }

        public ReadDataByIdentifier(ISO_TP.Message message)
            : base(message)
        {
            Identifier = (ushort)(message.Payload[1] << 8 | message.Payload[2]); // big endian conversion
            // we don't handle multiple identifiers at the moment
            if (message.PayloadSize > 3)
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()} : {Identifier}";
        }
    }
}
