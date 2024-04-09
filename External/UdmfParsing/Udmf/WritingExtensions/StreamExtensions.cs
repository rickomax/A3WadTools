// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace UdmfParsing.Udmf.WritingExtensions
{
    public static class StreamExtensions
    {
        public static void WriteLine(this Stream stream, string value)
        {
            var textBytes = Encoding.ASCII.GetBytes(value + "\n");

            stream.Write(textBytes, 0, textBytes.Length);
        }

        public static void WritePropertyVerbatim(this Stream stream, string name, string value, bool indent)
        {
            var indention = indent ? "\t" : String.Empty;
            WriteLine(stream, $"{indention}{name} = {value};");
        }

        public static void WriteProperty(this Stream stream, string name, string value, bool indent)
        {
            WritePropertyVerbatim(stream, name, $"\"{value}\"", indent);
        }

        public static void WriteProperty(this Stream stream, string name, int value, bool indent)
        {
            WritePropertyVerbatim(stream, name, value.ToString(CultureInfo.InvariantCulture), indent);
        }

        public static void WriteProperty(this Stream stream, string name, double value, bool indent)
        {
            string DoubleToString(double source) => (source % 1) == 0 ? source.ToString("f1") : source.ToString();

            WritePropertyVerbatim(stream, name, DoubleToString(value), indent);
        }

        public static void WriteProperty(this Stream stream, string name, bool value, bool indent)
        {
            WritePropertyVerbatim(stream, name, value.ToString().ToLowerInvariant(), indent);
        }
    }
}