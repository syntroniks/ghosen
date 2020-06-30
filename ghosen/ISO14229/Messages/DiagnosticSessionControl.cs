namespace ghosen.ISO14229.Messages
{
  public class DiagnosticSessionControl : ISO14229Message
  {
    public enum DiagnosticSessionType
    {
      DefaultSession = 0x01,
      ProgrammingSession = 0x02,
      ExtendedDiagnosticSession = 0x03,
      SafetySystemDiagnosticSession = 0x04
    }

    public DiagnosticSessionType SessionType { get; set; }

    public DiagnosticSessionControl(ISO_TP.Message message)
        : base(message)
    {
      SessionType = (DiagnosticSessionType)message.Payload[1];
      if (base.MessageType == ServiceMessageType.Request)
      {
      }
      else
      {
      }
    }
  }
}
