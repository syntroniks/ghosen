using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ghosen.ISO_TP;
using ghosen.ISO14229.Messages;

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

		public override string ToString()
		{
			return $"{Enum.GetName(typeof(ServiceType), Service)}{MessageType} : Len {Data.Length} : {{{Utils.ByteArrayToHexViaLookup32(Data)}}}";
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
          return new ReadDataByIdentifier(message);
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
          return new RoutineControl_SIMOS18(message);
          break;
        case ServiceType.RequestDownload:
          return new RequestDownload(message);
          break;
        case ServiceType.RequestUpload:
          return new RequestUpload(message);
          break;
        case ServiceType.TransferData:
          return new TransferData(message);
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

  public class MessageParser
  {
    public static IEnumerable<ISO14229Message> ProcessMessages(IEnumerable<ISO_TP.Message> messages)
    {
      foreach (var msg in messages)
      {
        // .Create tries to make a specific child class if possible.
        // Otherwise, it just makes the base class
        yield return ISO14229.ISO14229Message.Create(msg);
      }
      yield break;
    }
  }
}
