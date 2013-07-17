using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Coob
{
    class ZlibHelper
    {
        public static byte[] UncompressBuffer(byte[] buffer)
        {
            byte[] toDecompress = new byte[buffer.Length - 2];
            Array.Copy(buffer, 2, toDecompress, 0, toDecompress.Length);

            return DeflateStream.UncompressBuffer(toDecompress);
        }

        public static byte[] CompressBuffer(byte[] buffer)
        {
            return DeflateStream.CompressBuffer(buffer);
        }
    }
}
