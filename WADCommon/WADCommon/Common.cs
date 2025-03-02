using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WADCommon
{
    public static class Common
    {
        [DllImport("kernel32", EntryPoint = "GetShortPathName", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetShortPathName(string longPath, StringBuilder shortPath, int bufSize);

        public const float AckScale = 16f;
        public const float Scale = 1f / AckScale;

        public struct Point 
        {
            public float X;
            public float Y;

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }
        }

        public static string GetShortName(string sLongFileName)
        {
            var buffer = new StringBuilder(259);
            var len = GetShortPathName(sLongFileName, buffer, buffer.Capacity);
            if (len == 0)
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return buffer.ToString();
        }

        public static bool IsValidPath(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var filename = Path.GetFileName(path);
            var invalidChars = Path.GetInvalidFileNameChars();
            return directory != null && filename.IndexOfAny(invalidChars) < 0;
        }

        public static byte[] ReadFully(Stream input)
        {
            if (input is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }
            var buffer = new byte[16 * 1024];
            using (var outputMemoryStream = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputMemoryStream.Write(buffer, 0, read);
                }
                return outputMemoryStream.ToArray();
            }
        }

        public static short GetInt16Le(byte[] buffer, int offset)
        {
            return (short)(buffer[offset + 1] << 8 | buffer[offset]);
        }

        public static int GetInt32Le(byte[] buffer, int offset)
        {
            return buffer[offset + 3] << 24 | buffer[offset + 2] << 16 | buffer[offset + 1] << 8 | buffer[offset];
        }

        public static byte[] ScaleImage(byte[] imageData, int originalWidth, int originalHeight, int newWidth, int newHeight)
        {
            var scaledImage = new byte[newWidth * newHeight];
            var widthRatio = (float)originalWidth / newWidth;
            var heightRatio = (float)originalHeight / newHeight;

            for (var y = 0; y < newHeight; y++)
            {
                for (var x = 0; x < newWidth; x++)
                {
                    var nearestX = (int)Math.Floor(x * widthRatio);
                    var nearestY = (int)Math.Floor(y * heightRatio);

                    var originalIndex = nearestY * originalWidth + nearestX;
                    var scaledIndex = y * newWidth + x;

                    scaledImage[scaledIndex] = imageData[originalIndex];
                }
            }
            return scaledImage;
        }
    }
}
