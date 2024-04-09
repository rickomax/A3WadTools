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
    public sealed partial class Vertex
    {
        private bool _xHasBeenSet = false;
        private double _x;
        public double X
        {
            get { return _x; }
            set
            {
                _xHasBeenSet = true;
                _x = value;
            }
        }
        private bool _yHasBeenSet = false;
        private double _y;
        public double Y
        {
            get { return _y; }
            set
            {
                _yHasBeenSet = true;
                _y = value;
            }
        }
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

        public Vertex() { }
        public Vertex(
            double x,
            double y,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            X = x;
            Y = y;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            stream.WriteLine("vertex");
            stream.WriteLine("{");
            stream.WriteProperty("x", _x, indent: true);
            stream.WriteProperty("y", _y, indent: true);
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
            if (!_xHasBeenSet) throw new InvalidUdmfException("Did not set X on Vertex");
            if (!_yHasBeenSet) throw new InvalidUdmfException("Did not set Y on Vertex");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public Vertex Clone()
        {
            return new Vertex(
                x: X,
                y: Y,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

}
