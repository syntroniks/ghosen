namespace ghosen.ISO_TP
{
  public class Message
  {
    // May or may not be relevant at this layer, but we'll keep is just in case
    public ArbitrationId ArbId { get; set; }

    public byte[] Payload { get; set; }

    // The payload length may or may not represent the valid data size.
    public int PayloadSize { get; set; }

    // True if this message has been completed
    public bool Complete { get; set; }

    // True if this message will span multiple frames
    public bool MultiFrame { get; set; }
  }
}
