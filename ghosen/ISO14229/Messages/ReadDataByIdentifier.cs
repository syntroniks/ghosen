using System.Linq;

namespace ghosen.ISO14229.Messages
{
  public class ReadDataByIdentifier : ISO14229Message
  {
    public ushort Identifier { get; set; }
    public byte[] ResponseData { get; set; }

    public ReadDataByIdentifier(ISO_TP.Message message)
        : base(message)
    {
      Identifier = (ushort)(message.Payload[1] << 8 | message.Payload[2]); // big endian conversion

      if (base.MessageType == ServiceMessageType.Request)
      {
        // nbd
        // we don't handle multiple identifiers at the moment
      }
      else
      {
        // get response
        // skip service byte and identifier
        ResponseData = message.Payload.Skip(3).Take(message.PayloadSize - 3).ToArray();
      }
    }

    public override string ToString()
    {
      return $"{base.ToString()} : {Identifier}";
    }
  }
}
