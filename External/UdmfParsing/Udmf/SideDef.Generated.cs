// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UdmfParsing.Udmf.WritingExtensions;

namespace UdmfParsing.Udmf
{
    [GeneratedCode("UdmfParserGenerator", "1.0.0.0")]
    public sealed partial class SideDef
    {
        private bool _sectorHasBeenSet = false;
        private int _sector;
        public int Sector
        {
            get { return _sector; }
            set
            {
                _sectorHasBeenSet = true;
                _sector = value;
            }
        }
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;
        public string TextureTop { get; set; } = "-";
        public string TextureBottom { get; set; } = "-";
        public string TextureMiddle { get; set; } = "-";
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

        public SideDef() { }
        public SideDef(
            int sector,
            int offsetX = 0,
            int offsetY = 0,
            string textureTop = "-",
            string textureBottom = "-",
            string textureMiddle = "-",
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            Sector = sector;
            OffsetX = offsetX;
            OffsetY = offsetY;
            TextureTop = textureTop;
            TextureBottom = textureBottom;
            TextureMiddle = textureMiddle;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            stream.WriteLine("sidedef");
            stream.WriteLine("{");
            stream.WriteProperty("sector", _sector, indent: true);
            if (OffsetX != 0) stream.WriteProperty("offsetX", OffsetX, indent: true);
            if (OffsetY != 0) stream.WriteProperty("offsetY", OffsetY, indent: true);
            if (TextureTop != "-") stream.WriteProperty("textureTop", TextureTop, indent: true);
            if (TextureBottom != "-") stream.WriteProperty("textureBottom", TextureBottom, indent: true);
            if (TextureMiddle != "-") stream.WriteProperty("textureMiddle", TextureMiddle, indent: true);
            if (Comment != "") stream.WriteProperty("comment", Comment, indent: true);
            foreach (var property in UnknownProperties)
            {
                stream.WritePropertyVerbatim((string)property.Name, property.Value, indent: true);
            }
            stream.WriteLine("}");
            return stream;
        }

        public void CheckSemanticValidity()
        {
            if (!_sectorHasBeenSet) throw new InvalidUdmfException("Did not set Sector on SideDef");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public SideDef Clone()
        {
            return new SideDef(
                sector: Sector,
                offsetX: OffsetX,
                offsetY: OffsetY,
                textureTop: TextureTop,
                textureBottom: TextureBottom,
                textureMiddle: TextureMiddle,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

}
