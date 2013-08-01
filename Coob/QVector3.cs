using System;
using System.IO;

namespace Coob
{
    public struct QVector3 : ICloneable
    {
        public long X, Y, Z;

        public QVector3 Clone()
        {
            return new QVector3(X, Y, Z);
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
}
