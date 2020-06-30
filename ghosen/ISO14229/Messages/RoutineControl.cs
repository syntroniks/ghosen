using System;
using System.Linq;

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
      RoutineIdentifier = (ushort)(message.Payload[2] << 8 | message.Payload[3]); // big endian conversion
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

  public class RoutineControl_SIMOS18 : RoutineControl
  {
    public enum SIMOS18_RoutineIdentifier
    {
      SIMOS18_1_ERASE_SEGMENT = 0xFF00,
      SIMOS18_1_VALIDATE_SEGMENT = 0x0202,
      SIMOS18_1_START_FLASH = 0x0203,
    };

    new public SIMOS18_RoutineIdentifier RoutineIdentifier { get; set; }


    public RoutineControl_SIMOS18(ISO_TP.Message message)
        : base(message)
    {
      this.RoutineIdentifier = (SIMOS18_RoutineIdentifier)base.RoutineIdentifier;
      var a = RoutineInformation.Parse(this.MessageType, this.RoutineIdentifier, this.RoutineControlOptionRecord);
    }

    public class RoutineInformation
    {
      public static RoutineInformation Parse(ServiceMessageType msgType, SIMOS18_RoutineIdentifier routineIdentifier, byte[] options)
      {
        RoutineInformation ret = null;

        switch (routineIdentifier)
        {
          case SIMOS18_RoutineIdentifier.SIMOS18_1_ERASE_SEGMENT:
            ret = new EraseSegmentRoutine(msgType, options);
            break;
          case SIMOS18_RoutineIdentifier.SIMOS18_1_VALIDATE_SEGMENT:
            ret = new ValidateSegmentRoutine(msgType, options);
            break;
          case SIMOS18_RoutineIdentifier.SIMOS18_1_START_FLASH:
            ret = new StartFlashRoutine();
            break;
          default:
            break;
        }

        return ret;
      }
    }

    public class EraseSegmentRoutine : RoutineInformation
    {
      public int? SegmentID { get; set; }
      // parse segment number
      public EraseSegmentRoutine(ServiceMessageType msgType, byte[] options)
          : base()
      {
        if (msgType == ServiceMessageType.Request)
        {
          SegmentID = options[1];
        }
      }
    }

    public class ValidateSegmentRoutine : RoutineInformation
    {
      public int? SegmentID { get; set; }
      // parse segment number
      public ValidateSegmentRoutine(ServiceMessageType msgType, byte[] options)
          : base()
      {
        if (msgType == ServiceMessageType.Request)
        {
          SegmentID = options[1];
        }
      }
    }

    public class StartFlashRoutine : RoutineInformation
    {
      // nothing
    }
  }
}
