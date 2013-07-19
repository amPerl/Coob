using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coob
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, Vector3 vec3)
        {
            writer.Write(vec3.X);
            writer.Write(vec3.Y);
            writer.Write(vec3.Z);
        }

        public static void Write(this BinaryWriter writer, QVector3 qvec3)
        {
            writer.Write(qvec3.X);
            writer.Write(qvec3.Y);
            writer.Write(qvec3.Z);
        }

        public static void Pad(this BinaryWriter writer, int length)
        {
            var nullArray = new byte[length];
            writer.Write(nullArray);
        }
    }
}