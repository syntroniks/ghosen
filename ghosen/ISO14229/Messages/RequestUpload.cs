using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ghosen.ISO14229.Messages
{
    public class RequestUpload : RequestDownload
    {
        public RequestUpload(ISO_TP.Message message)
            : base(message)
        {
        }
    }
}
