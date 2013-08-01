using System;
using System.IO;
using System.Net.Sockets;

namespace Coob
{
    public class NetReader : BinaryReader
    {
	    readonly NetworkStream ns;
	    readonly byte[] tempBuffer;

        public NetReader(NetworkStream stream) : 
            base(stream)
        {
            ns = stream;
            tempBuffer = new byte[8];
        }

        public override byte ReadByte()
        {
            byte val = (byte)ns.ReadByte();
            return val;
        }

        public override byte[] ReadBytes(int n)
        {
            byte[] bytes = new byte[n];
            ns.Read(bytes, 0, n);
            return bytes;
        }

        public override short ReadInt16()
        {
            ns.Read(tempBuffer, 0, 2);
            return BitConverter.ToInt16(tempBuffer, 0);
        }

        public override ushort ReadUInt16()
        {
            ns.Read(tempBuffer, 0, 2);
            return BitConverter.ToUInt16(tempBuffer, 0);
        }

        public override int ReadInt32()
        {
            ns.Read(tempBuffer, 0, 4);
            return BitConverter.ToInt32(tempBuffer, 0);
        }

        public override uint ReadUInt32()
        {
            ns.Read(tempBuffer, 0, 4);
            return BitConverter.ToUInt32(tempBuffer, 0);
        }

        public override float ReadSingle()
        {
            ns.Read(tempBuffer, 0, 4);
            return BitConverter.ToSingle(tempBuffer, 0);
        }

        public QVector3 ReadQVector3()
        {
            return new QVector3
                   {
                       X = ReadInt64(),
                       Y = ReadInt64(),
                       Z = ReadInt64(),
                   };
        }

        public Vector3 ReadVector3()
        {
            return new Vector3
            {
                X = ReadSingle(),
                Y = ReadSingle(),
                Z = ReadSingle()
            };
        }

        public override long ReadInt64()
        {
            ns.Read(tempBuffer, 0, 8);
            return BitConverter.ToInt64(tempBuffer, 0);
        }
    }
}
