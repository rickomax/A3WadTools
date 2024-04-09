// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UdmfParsing.Common;
using UdmfParsing.Udmf.WritingExtensions;

namespace UdmfParsing.Udmf
{
    public sealed class UnknownBlock
    {
        public Identifier Name { get; }
        public List<UnknownProperty> Properties { get; } = new List<UnknownProperty>();

        public UnknownBlock(Identifier name)
        {
            Name = name;
        }

        public Stream WriteTo(Stream stream)
        {
            stream.WriteLine((string)Name);
            stream.WriteLine("{");
            foreach (var property in Properties)
            {
                stream.WritePropertyVerbatim((string)property.Name, property.Value, indent: true);
            }
            stream.WriteLine("}");

            return stream;
        }

        public UnknownBlock Clone()
        {
            var block = new UnknownBlock(Name);
            block.Properties.AddRange(Properties.Select(p => p.Clone()));

            return block;
        }
    }
}