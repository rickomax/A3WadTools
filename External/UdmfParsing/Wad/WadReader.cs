// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UdmfParsing.Wad.StreamExtensions;

namespace UdmfParsing.Wad
{
    public sealed class WadReader : IDisposable
    {
        private readonly Stream _stream;

        public ImmutableArray<LumpInfo> Directory { get; }

        public IEnumerable<string> GetMapNames() =>
            from lump in Directory
            let name = lump.Name.ToString()
            where Regex.IsMatch(name, @"^(MAP\d{2}|E\dM\d)$")
            select name;

        private WadReader(Stream stream, ImmutableArray<LumpInfo> lumps)
        {
            _stream = stream;
            Directory = lumps;
        }

        public static WadReader Read(string filePath)
        {
            var stream = File.OpenRead(filePath);

            // TODO: More graceful handling of invalid files.

            var identification = stream.ReadText(4);
            var numLumps = stream.ReadInt();
            var directoryPosition = stream.ReadInt();

            stream.Position = directoryPosition;

            var directory =
                Enumerable.Range(1, numLumps).
                Select(_ => stream.ReadLumpMetadata())
                .ToImmutableArray();

            return new WadReader(stream, directory);
        }

        public Stream GetLumpStream(LumpInfo info) => new ReadOnlySubStream(_stream, position: info.Position, length: info.Size);

        public Stream GetMapStream(string mapName)
        {
            var indexOfMarker = Directory.Select((info, index) => (info, index))
                .Single(pair => pair.info.Name == mapName).index;

            return GetLumpStream(Directory[indexOfMarker + 1]);
        }

        public void Dispose() => _stream.Dispose();
    }
}
