using System;
using System.Linq;

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

    public int BlockSize { get; set; }

    public byte[] DataRecord { get; set; }

    public TransferData(ISO_TP.Message message)
        : base(message)
    {
      BlockSize = message.PayloadSize - 2;
      BlockSequenceCounter = message.Payload[1];
      DataRecord = message.Payload.Skip(2).Take(BlockSize).ToArray();
      if (base.MessageType == ServiceMessageType.Request)
      {
      }
      else
      {
      }

    }

    public override string ToString()
    {
      var snippedData = Utils.ByteArrayToHexViaLookup32(DataRecord.Take(16)) + "..." + Utils.ByteArrayToHexViaLookup32(DataRecord.TakeLast(16));
      return $"{Enum.GetName(typeof(ServiceType), Service)}{MessageType}: {BlockSequenceCounter} : {BlockSize} : [{snippedData}]";
    }
  }
}
