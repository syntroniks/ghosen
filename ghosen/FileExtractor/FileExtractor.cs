using System.Collections.Generic;
using System.Linq;

namespace ghosen.FileExtractor
{
    public class FlashBlock
    {
        public uint MemoryAddress { get; set; }

        public uint Compression { get; set; }

        public byte[] PayLoad { get; set; }

        public FlashBlock(uint memory_address, uint compression, byte[] payload)
        {
            PayLoad = payload.ToArray();
            MemoryAddress = memory_address;
            Compression = compression;
        }
    }

    internal static class FileExtractor
    {
        internal static IEnumerable<FlashBlock> ProcessMessages(IEnumerable<ISO14229.ISO14229Message> messages)
        {
            var ret = new List<byte>();
            bool collectingBytes = false;
            uint memoryAddress = 0;
            uint compression = 0;
            foreach (var msg in messages)
            {
                // todo: be more careful with transferdataresponses, other responses, etc.
                if (collectingBytes)
                {
                    if (msg.Service == ISO14229.ServiceType.TransferData)
                    {

                        ret.AddRange(((ISO14229.Messages.TransferData)msg).DataRecord);
                    }
                    else if (msg.Service == ISO14229.ServiceType.RequestTransferExit)
                    {
                        collectingBytes = false;
                        var copy = ret.ToArray();
                        ret.Clear();
                        yield return new FlashBlock(memoryAddress, compression, copy);
                    }
                }
                else
                {
                    if (msg.Service == ISO14229.ServiceType.RequestDownload)
                    {
                        collectingBytes = true;
                        memoryAddress = ((ISO14229.Messages.RequestDownload)msg).MemoryAddress;
                        compression = (uint)((ISO14229.Messages.RequestDownload)msg).Compression;
                    }
                }
            }
            yield break;
        }
    }
}