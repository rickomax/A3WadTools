using System.IO;

namespace WADCommon
{
    public static class Common
    {

        public const float Scale = 1f / 16f;

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
    }
}
