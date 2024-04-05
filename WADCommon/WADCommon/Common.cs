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

        public static ushort CalculateHash(string input)
        {
            var hash = 0;
            foreach (var c in input)
            {
                hash = c + (hash << 6) + (hash << 16) - hash;
            }
            return (ushort)(hash & 0xFFFF);
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
    }
}
