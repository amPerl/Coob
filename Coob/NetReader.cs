using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Coob
{
    public struct Vector3 : ICloneable
    {
        private float _x;
        private float _y;
        private float _z;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Pitch //Alias of X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Roll //Alias of Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public float Yaw //Alias of X
        {
            get { return _z; }
            set { _z = value; }
        }

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

        public override string ToString()
        {
            return "Vec3f{ " + X + ", " + Y + ", " + Z + " }";
        }

        object ICloneable.Clone()
        {
            return new Vector3() { X = this.X, Y = this.Y, Z = this.Z };
        }
    }

    public struct QVector3 : ICloneable
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

        public override string ToString()
        {
            return "QVec3f{ " + X + ", " + Y + ", " + Z + " }";
        }

        object ICloneable.Clone()
        {
            return new Vector3() { X = this.X, Y = this.Y, Z = this.Z };
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
            tempBuffer = new byte[8];
            ns.Read(tempBuffer, 0, 8);
            return BitConverter.ToInt64(tempBuffer, 0);
        }
    }
}
