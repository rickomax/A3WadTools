// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace UdmfParsing.Udmf.Parsing.TotallyCustom
{
    public struct FilePosition
    {
        public int Line { get; }
        public int Column { get; }

        private FilePosition(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public static FilePosition StartOfFile() => new FilePosition(1, 1);
        public FilePosition NextChar() => new FilePosition(Line, Column + 1);
        public FilePosition NextLine() => new FilePosition(Line + 1, 1);

        public override string ToString() => $"Line: {Line}, Col: {Column}";
    }
}
