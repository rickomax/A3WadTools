// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using UdmfParsing.Udmf;
using UdmfParsing.Wad.StreamExtensions;

namespace UdmfParsing.Wad
{
    public sealed class WadWriter
    {
        private readonly List<ILump> _lumps = new List<ILump>();

        public void Append(string name, MapData map)
        {
            Append(new Marker(name));
            Append(new UdmfLump("TEXTMAP", map));
            Append(new Marker("ENDMAP"));
        }
        public void Append(ILump lump) => _lumps.Add(lump);

        public void SaveTo(Stream stream)
        {
            stream.WriteText("PWAD");
            stream.WriteInt(_lumps.Count);

            // Fill in this position after writing the data
            int directoryOffsetPosition = (int)stream.Position;
            stream.Position += 4;

            var metadata = new List<LumpInfo>();
            foreach (var lump in _lumps)
            {
                int startOfLump = (int)stream.Position;

                lump.WriteTo(stream);

                metadata.Add(new LumpInfo(
                    position: startOfLump,
                    size: (int)stream.Position - startOfLump,
                    name: lump.Name));
            }

            int startOfDirectory = (int)stream.Position;

            // Write directory
            foreach (var lumpMetadata in metadata)
            {
                lumpMetadata.WriteTo(stream);
            }

            // Go back and set the directory position
            stream.Position = directoryOffsetPosition;
            stream.WriteInt(startOfDirectory);
        }

        public void SaveTo(string filePath)
        {
            using (var fs = File.Open(filePath, FileMode.Create))
            {
                SaveTo(fs);
            }
        }
    }
}
