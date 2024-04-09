// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;
using System.Text;

namespace UdmfParsing.Wad.StreamExtensions
{
    public static class Extensions
    {
        public static void WriteInt(this Stream stream, int value)
        {
            stream.WriteArray(BitConverter.GetBytes(value));
        }

        public static void WriteText(this Stream stream, string text)
        {
            WriteText(stream, text, text.Length);
        }

        public static void WriteText(this Stream stream, string text, int totalLength)
        {
            WriteArray(stream, Encoding.ASCII.GetBytes(text), totalLength);
        }

        public static void WriteArray(this Stream stream, byte[] bytes)
        {
            WriteArray(stream, bytes, bytes.Length);
        }

        public static void WriteArray(this Stream stream, byte[] bytes, int totalLength)
        {
            stream.Write(bytes, 0, bytes.Length);
            var padding = totalLength - bytes.Length;
            if (padding != 0)
            {
                stream.Write(new byte[padding], 0, padding);
            }
        }

        public static void WriteTo(this LumpInfo metaData, Stream stream)
        {
            stream.WriteInt(metaData.Position);
            stream.WriteInt(metaData.Size);
            stream.WriteText(metaData.Name.ToString(), totalLength: LumpName.MaxLength);
        }

        public static string ReadText(this Stream stream, int length)
        {
            return Encoding.ASCII.GetString(stream.ReadArray(length));
        }

        public static int ReadInt(this Stream stream)
        {
            return BitConverter.ToInt32(stream.ReadArray(4), 0);
        }

        public static byte[] ReadArray(this Stream stream, int length)
        {
            var data = new byte[length];
            stream.Read(data, 0, length);
            return data;
        }

        public static LumpInfo ReadLumpMetadata(this Stream stream)
        {
            return new LumpInfo(
                position: stream.ReadInt(),
                size: stream.ReadInt(),
                name: stream.ReadText(LumpName.MaxLength).TrimEnd((char)0));
        }
    }
}
