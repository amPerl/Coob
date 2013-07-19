using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Coob
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector3
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(0)]
        public float Pitch;

        [FieldOffset(4)]
        public float Y;
        [FieldOffset(4)]
        public float Roll;

        [FieldOffset(8)]
        public float Z;
        [FieldOffset(8)]
        public float Yaw;

        public Vector3 Clone()
        {
            return new Vector3() { X = this.X, Y = this.Y, Z = this.Z };
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }
    }

    public struct QVector3
    {
        public long X, Y, Z;

        public QVector3 Clone()
        {
            return new QVector3() { X = this.X, Y = this.Y, Z = this.Z };
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }
    }

    public class NetReader : BinaryReader
    {
        NetworkStream ns;
        byte[] tempBuffer;

        public NetReader(NetworkStream stream) : 
            base(stream)
        {
            ns = stream;
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
            tempBuffer = new byte[2];
            ns.Read(tempBuffer, 0, 2);
            return BitConverter.ToInt16(tempBuffer, 0);
        }

        public override ushort ReadUInt16()
        {
            tempBuffer = new byte[2];
            ns.Read(tempBuffer, 0, 2);
            return BitConverter.ToUInt16(tempBuffer, 0);
        }

        public override int ReadInt32()
        {
            tempBuffer = new byte[4];
            ns.Read(tempBuffer, 0, 4);
            return BitConverter.ToInt32(tempBuffer, 0);
        }

        public override uint ReadUInt32()
        {
            tempBuffer = new byte[4];
            ns.Read(tempBuffer, 0, 4);
            return BitConverter.ToUInt32(tempBuffer, 0);
        }

        public override float ReadSingle()
        {
            tempBuffer = new byte[4];
            ns.Read(tempBuffer, 0, 4);
            return BitConverter.ToSingle(tempBuffer, 0);
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
            tempBuffer = new byte[8];
            ns.Read(tempBuffer, 0, 8);
            return BitConverter.ToInt64(tempBuffer, 0);
        }
    }
}
