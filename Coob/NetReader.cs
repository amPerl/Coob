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
        private float x;
        private float y;
        private float z;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Pitch //Alias of X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Roll //Alias of Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public float Yaw //Alias of Z
        {
            get { return z; }
            set { z = value; }
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(float xyz)
        {
            x = xyz;
            y = xyz;
            z = xyz;
        }

        public Vector3 Clone()
        {
            return new Vector3(x, y, z);
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
            return new Vector3(x, y, z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3 operator *(float a, Vector3 b)
        {
            return b * a;
        }
    }

    public struct QVector3 : ICloneable
    {
        public long X, Y, Z;

        public QVector3 Clone()
        {
            return new QVector3() { X = this.X, Y = this.Y, Z = this.Z };
        }

        public QVector3(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public QVector3 (long xyz)
        {
            X = xyz;
            Y = xyz;
            Z = xyz;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public override string ToString()
        {
            return "QVec3L{ " + X + ", " + Y + ", " + Z + " }";
        }

        object ICloneable.Clone()
        {
            return new Vector3(X, Y, Z);
        }

        public static QVector3 operator +(QVector3 a, QVector3 b)
        {
            return new QVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static QVector3 operator -(QVector3 a, QVector3 b)
        {
            return new QVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static QVector3 operator *(QVector3 a, QVector3 b)
        {
            return new QVector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static QVector3 operator *(QVector3 a, float b)
        {
            return new QVector3((long)(a.X * b), (long)(a.Y * b), (long)(a.Z * b));
        }

        public static QVector3 operator *(float a, QVector3 b)
        {
            return b * a;
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
