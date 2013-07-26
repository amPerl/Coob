using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

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
            byte[] compressed;
            using (var input = new MemoryStream(buffer))
            using (var compressStream = new MemoryStream())
            using (var compressor = new ZlibStream(compressStream, CompressionMode.Compress, CompressionLevel.Default, true))
            {
                input.CopyTo(compressor);
                compressor.Close();
                compressed = compressStream.ToArray();
            }
            return compressed;
        }
    }
}
