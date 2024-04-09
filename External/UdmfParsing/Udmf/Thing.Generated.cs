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
    public sealed partial class Thing
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
        private bool _typeHasBeenSet = false;
        private int _type;
        public int Type
        {
            get { return _type; }
            set
            {
                _typeHasBeenSet = true;
                _type = value;
            }
        }
        public int Id { get; set; } = 0;
        public double Height { get; set; } = 0;
        public int Angle { get; set; } = 0;
        public bool Skill1 { get; set; } = false;
        public bool Skill2 { get; set; } = false;
        public bool Skill3 { get; set; } = false;
        public bool Skill4 { get; set; } = false;
        public bool Skill5 { get; set; } = false;
        public bool Ambush { get; set; } = false;
        public bool Single { get; set; } = false;
        public bool Dm { get; set; } = false;
        public bool Coop { get; set; } = false;
        public string Comment { get; set; } = "";
        public List<UnknownProperty> UnknownProperties { get; } = new List<UnknownProperty>();

        public Thing() { }
        public Thing(
            double x,
            double y,
            int type,
            int id = 0,
            double height = 0,
            int angle = 0,
            bool skill1 = false,
            bool skill2 = false,
            bool skill3 = false,
            bool skill4 = false,
            bool skill5 = false,
            bool ambush = false,
            bool single = false,
            bool dm = false,
            bool coop = false,
            string comment = "",
            IEnumerable<UnknownProperty> unknownProperties = null)
        {
            X = x;
            Y = y;
            Type = type;
            Id = id;
            Height = height;
            Angle = angle;
            Skill1 = skill1;
            Skill2 = skill2;
            Skill3 = skill3;
            Skill4 = skill4;
            Skill5 = skill5;
            Ambush = ambush;
            Single = single;
            Dm = dm;
            Coop = coop;
            Comment = comment;
            UnknownProperties.AddRange(unknownProperties ?? Enumerable.Empty<UnknownProperty>());
            AdditionalSemanticChecks();
        }

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();
            stream.WriteLine("thing");
            stream.WriteLine("{");
            stream.WriteProperty("x", _x, indent: true);
            stream.WriteProperty("y", _y, indent: true);
            stream.WriteProperty("type", _type, indent: true);
            if (Id != 0) stream.WriteProperty("id", Id, indent: true);
            if (Height != 0) stream.WriteProperty("height", Height, indent: true);
            if (Angle != 0) stream.WriteProperty("angle", Angle, indent: true);
            if (Skill1 != false) stream.WriteProperty("skill1", Skill1, indent: true);
            if (Skill2 != false) stream.WriteProperty("skill2", Skill2, indent: true);
            if (Skill3 != false) stream.WriteProperty("skill3", Skill3, indent: true);
            if (Skill4 != false) stream.WriteProperty("skill4", Skill4, indent: true);
            if (Skill5 != false) stream.WriteProperty("skill5", Skill5, indent: true);
            if (Ambush != false) stream.WriteProperty("ambush", Ambush, indent: true);
            if (Single != false) stream.WriteProperty("single", Single, indent: true);
            if (Dm != false) stream.WriteProperty("dm", Dm, indent: true);
            if (Coop != false) stream.WriteProperty("coop", Coop, indent: true);
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
            if (!_xHasBeenSet) throw new InvalidUdmfException("Did not set X on Thing");
            if (!_yHasBeenSet) throw new InvalidUdmfException("Did not set Y on Thing");
            if (!_typeHasBeenSet) throw new InvalidUdmfException("Did not set Type on Thing");
            AdditionalSemanticChecks();
        }

        partial void AdditionalSemanticChecks();

        public Thing Clone()
        {
            return new Thing(
                x: X,
                y: Y,
                type: Type,
                id: Id,
                height: Height,
                angle: Angle,
                skill1: Skill1,
                skill2: Skill2,
                skill3: Skill3,
                skill4: Skill4,
                skill5: Skill5,
                ambush: Ambush,
                single: Single,
                dm: Dm,
                coop: Coop,
                comment: Comment,
                unknownProperties: UnknownProperties.Select(item => item.Clone()));
        }
    }

}
