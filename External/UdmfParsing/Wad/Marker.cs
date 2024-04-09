// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;

namespace UdmfParsing.Wad
{
    public sealed class Marker : ILump
    {
        public LumpName Name { get; }

        public Marker(LumpName name)
        {
            Name = name;
        }

        public void WriteTo(Stream stream)
        {
            // Do nothing; no data
        }
    }
}
