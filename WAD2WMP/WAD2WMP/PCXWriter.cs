using System.IO;

namespace WAD2WMP
{
    public class PCXWriter
    {
        private static void WriteScanLineRLE(BinaryWriter binaryWriter, byte[] scanline)
        {
            var previousByte = -1;
            var repeatCount = 0;
            for (var i = 0; i < scanline.Length; i++)
            {
                if ((scanline[i] & 0xff) == previousByte && repeatCount < 63)
                {
                    ++repeatCount;
                }
                else
                {
                    if (repeatCount > 0)
                    {
                        if (repeatCount == 1 && (previousByte & 0xc0) != 0xc0)
                        {
                            binaryWriter.Write((byte)previousByte);
                        }
                        else
                        {
                            binaryWriter.Write((byte)(0xc0 | repeatCount));
                            binaryWriter.Write((byte)previousByte);
                        }
                    }
                    previousByte = 0xff & scanline[i];
                    repeatCount = 1;
                }
            }
            if (repeatCount > 0)
            {
                if (repeatCount == 1 && (previousByte & 0xc0) != 0xc0)
                {
                    binaryWriter.Write((byte)previousByte);
                }
                else
                {
                    binaryWriter.Write((byte)(0xc0 | repeatCount));
                    binaryWriter.Write((byte)previousByte);
                }
            }
        }

        public static void WritePCX(byte[] src, int width, int height, Color[] palette, BinaryWriter binaryWriter)
        {
            var bytesPerLine = width % 2 == 0 ? width : width + 1;
            // PCX header
            binaryWriter.Write((byte)10); // manufacturer
            binaryWriter.Write((byte)5); // version
            binaryWriter.Write((byte)0); // encoding (RLE)
            binaryWriter.Write((byte)8); // bits per pixel
            binaryWriter.Write((ushort)0); // xMin
            binaryWriter.Write((ushort)0); // yMin
            binaryWriter.Write((ushort)(width - 1)); // xMax
            binaryWriter.Write((ushort)(height - 1)); // yMax
            binaryWriter.Write((ushort)72); // hDpi
            binaryWriter.Write((ushort)72); // vDpi
            binaryWriter.Write(new byte[48]); // 16 color palette
            binaryWriter.Write((byte)0); // reserved
            binaryWriter.Write((byte)1); // planes
            binaryWriter.Write((ushort)bytesPerLine); // bytes per line
            binaryWriter.Write((ushort)1); // palette info
            binaryWriter.Write((ushort)0); // hScreenSize
            binaryWriter.Write((ushort)0); // vScreenSize
            binaryWriter.Write(new byte[54]);
            var indices = new byte[bytesPerLine];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = src[y * width + x];
                    indices[x] = index;
                }
                binaryWriter.Write(indices);
                //WriteScanLineRLE(binaryWriter, indeces);
            }
            // palette
            binaryWriter.Write((byte)12);
            for (var i = 0; i < 256; i++)
            {
                var rgb = i < palette.Length ? palette[i] : default;
                binaryWriter.Write(rgb.R);
                binaryWriter.Write(rgb.G);
                binaryWriter.Write(rgb.B);
            }
        }
    }
}