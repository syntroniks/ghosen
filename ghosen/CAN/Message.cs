using System;
using System.Linq;

namespace ghosen.CAN
{
    public class Message
    {
        private uint _arbId;

        public uint ArbId
        {
            get { return _arbId; }
            set { _arbId = value; }
        }

        private byte[] _dataBytes;

        public byte[] RawData
        {
            get { return _dataBytes; }
            set { _dataBytes = value; }
        }

        public Message()
        {
            RawData = new byte[0];
        }

        public override string ToString()
        {
            return $@"{String.Format("{0:X3}", ArbId)}#{Utils.ByteArrayToHexViaLookup32(RawData)}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: handle null RawData
            var castObj = (Message)obj;

            return (ArbId == castObj.ArbId &&
                    RawData.SequenceEqual(castObj.RawData));
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode ^= ArbId.GetHashCode();
            hashCode ^= RawData.GetHashCode();
            return hashCode;
        }
    }
}
