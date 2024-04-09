// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;

namespace UdmfParsing.Wad
{
    public interface ILump
    {
        LumpName Name { get; }

        void WriteTo(Stream stream);
    }
}