// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.CodeDom.Compiler;
using UdmfParsing.Udmf.Parsing.TotallyCustom.AbstractSyntaxTree;

namespace UdmfParsing.Udmf.Parsing.TotallyCustom
{
    [GeneratedCode("UdmfParserGenerator", "1.0.0.0")]
    public static partial class UdmfSemanticAnalyzer
    {
        static partial void ProcessGlobalAssignment(MapData map, Assignment assignment)
        {
            switch (assignment.Name.ToLower())
            {
                case "namespace":
                    map.NameSpace = ReadStringValue(assignment, "MapData.NameSpace");
                    break;
                case "comment":
                    map.Comment = ReadStringValue(assignment, "MapData.Comment");
                    break;
                default:
                    map.UnknownProperties.Add(new UnknownProperty(assignment.Name, assignment.ValueAsString()));
                    break;
            }
        }

        static partial void ProcessBlock(MapData map, Block block)
        {
            switch (block.Name.ToLower())
            {
                case "linedef":
                    map.LineDefs.Add(ProcessLineDef(block));
                    break;
                case "sidedef":
                    map.SideDefs.Add(ProcessSideDef(block));
                    break;
                case "vertex":
                    map.Vertices.Add(ProcessVertex(block));
                    break;
                case "sector":
                    map.Sectors.Add(ProcessSector(block));
                    break;
                case "thing":
                    map.Things.Add(ProcessThing(block));
                    break;
                default:
                    map.UnknownBlocks.Add(ProcessUnknownBlock(block));
                    break;
            }
        }

        static LineDef ProcessLineDef(Block block)
        {
            var lineDef = new LineDef();
            foreach (var assignment in block.Fields)
            {
                switch (assignment.Name.ToLower())
                {
                    case "id":
                        lineDef.Id = ReadIntValue(assignment, "LineDef.Id");
                        break;
                    case "v1":
                        lineDef.V1 = ReadIntValue(assignment, "LineDef.V1");
                        break;
                    case "v2":
                        lineDef.V2 = ReadIntValue(assignment, "LineDef.V2");
                        break;
                    case "blocking":
                        lineDef.Blocking = ReadBoolValue(assignment, "LineDef.Blocking");
                        break;
                    case "blockmonsters":
                        lineDef.BlockMonsters = ReadBoolValue(assignment, "LineDef.BlockMonsters");
                        break;
                    case "twosided":
                        lineDef.TwoSided = ReadBoolValue(assignment, "LineDef.TwoSided");
                        break;
                    case "dontpegtop":
                        lineDef.DontPegTop = ReadBoolValue(assignment, "LineDef.DontPegTop");
                        break;
                    case "dontpegbottom":
                        lineDef.DontPegBottom = ReadBoolValue(assignment, "LineDef.DontPegBottom");
                        break;
                    case "secret":
                        lineDef.Secret = ReadBoolValue(assignment, "LineDef.Secret");
                        break;
                    case "blocksound":
                        lineDef.BlockSound = ReadBoolValue(assignment, "LineDef.BlockSound");
                        break;
                    case "dontdraw":
                        lineDef.DontDraw = ReadBoolValue(assignment, "LineDef.DontDraw");
                        break;
                    case "mapped":
                        lineDef.Mapped = ReadBoolValue(assignment, "LineDef.Mapped");
                        break;
                    case "special":
                        lineDef.Special = ReadIntValue(assignment, "LineDef.Special");
                        break;
                    case "arg0":
                        lineDef.Arg0 = ReadIntValue(assignment, "LineDef.Arg0");
                        break;
                    case "arg1":
                        lineDef.Arg1 = ReadIntValue(assignment, "LineDef.Arg1");
                        break;
                    case "arg2":
                        lineDef.Arg2 = ReadIntValue(assignment, "LineDef.Arg2");
                        break;
                    case "arg3":
                        lineDef.Arg3 = ReadIntValue(assignment, "LineDef.Arg3");
                        break;
                    case "arg4":
                        lineDef.Arg4 = ReadIntValue(assignment, "LineDef.Arg4");
                        break;
                    case "sidefront":
                        lineDef.SideFront = ReadIntValue(assignment, "LineDef.SideFront");
                        break;
                    case "sideback":
                        lineDef.SideBack = ReadIntValue(assignment, "LineDef.SideBack");
                        break;
                    case "comment":
                        lineDef.Comment = ReadStringValue(assignment, "LineDef.Comment");
                        break;
                    default:
                        lineDef.UnknownProperties.Add(new UnknownProperty(assignment.Name, assignment.ValueAsString()));
                        break;
                }
            }
            return lineDef;
        }

        static SideDef ProcessSideDef(Block block)
        {
            var sideDef = new SideDef();
            foreach (var assignment in block.Fields)
            {
                switch (assignment.Name.ToLower())
                {
                    case "offsetx":
                        sideDef.OffsetX = ReadIntValue(assignment, "SideDef.OffsetX");
                        break;
                    case "offsety":
                        sideDef.OffsetY = ReadIntValue(assignment, "SideDef.OffsetY");
                        break;
                    case "texturetop":
                        sideDef.TextureTop = ReadStringValue(assignment, "SideDef.TextureTop");
                        break;
                    case "texturebottom":
                        sideDef.TextureBottom = ReadStringValue(assignment, "SideDef.TextureBottom");
                        break;
                    case "texturemiddle":
                        sideDef.TextureMiddle = ReadStringValue(assignment, "SideDef.TextureMiddle");
                        break;
                    case "sector":
                        sideDef.Sector = ReadIntValue(assignment, "SideDef.Sector");
                        break;
                    case "comment":
                        sideDef.Comment = ReadStringValue(assignment, "SideDef.Comment");
                        break;
                    default:
                        sideDef.UnknownProperties.Add(new UnknownProperty(assignment.Name, assignment.ValueAsString()));
                        break;
                }
            }
            return sideDef;
        }

        static Vertex ProcessVertex(Block block)
        {
            var vertex = new Vertex();
            foreach (var assignment in block.Fields)
            {
                switch (assignment.Name.ToLower())
                {
                    case "x":
                        vertex.X = ReadDoubleValue(assignment, "Vertex.X");
                        break;
                    case "y":
                        vertex.Y = ReadDoubleValue(assignment, "Vertex.Y");
                        break;
                    case "comment":
                        vertex.Comment = ReadStringValue(assignment, "Vertex.Comment");
                        break;
                    default:
                        vertex.UnknownProperties.Add(new UnknownProperty(assignment.Name, assignment.ValueAsString()));
                        break;
                }
            }
            return vertex;
        }

        static Sector ProcessSector(Block block)
        {
            var sector = new Sector();
            foreach (var assignment in block.Fields)
            {
                switch (assignment.Name.ToLower())
                {
                    case "heightfloor":
                        sector.HeightFloor = ReadIntValue(assignment, "Sector.HeightFloor");
                        break;
                    case "heightceiling":
                        sector.HeightCeiling = ReadIntValue(assignment, "Sector.HeightCeiling");
                        break;
                    case "texturefloor":
                        sector.TextureFloor = ReadStringValue(assignment, "Sector.TextureFloor");
                        break;
                    case "textureceiling":
                        sector.TextureCeiling = ReadStringValue(assignment, "Sector.TextureCeiling");
                        break;
                    case "lightlevel":
                        sector.LightLevel = ReadIntValue(assignment, "Sector.LightLevel");
                        break;
                    case "special":
                        sector.Special = ReadIntValue(assignment, "Sector.Special");
                        break;
                    case "id":
                        sector.Id = ReadIntValue(assignment, "Sector.Id");
                        break;
                    case "comment":
                        sector.Comment = ReadStringValue(assignment, "Sector.Comment");
                        break;
                    default:
                        sector.UnknownProperties.Add(new UnknownProperty(assignment.Name, assignment.ValueAsString()));
                        break;
                }
            }
            return sector;
        }

        static Thing ProcessThing(Block block)
        {
            var thing = new Thing();
            foreach (var assignment in block.Fields)
            {
                switch (assignment.Name.ToLower())
                {
                    case "id":
                        thing.Id = ReadIntValue(assignment, "Thing.Id");
                        break;
                    case "x":
                        thing.X = ReadDoubleValue(assignment, "Thing.X");
                        break;
                    case "y":
                        thing.Y = ReadDoubleValue(assignment, "Thing.Y");
                        break;
                    case "height":
                        thing.Height = ReadDoubleValue(assignment, "Thing.Height");
                        break;
                    case "angle":
                        thing.Angle = ReadIntValue(assignment, "Thing.Angle");
                        break;
                    case "type":
                        thing.Type = ReadIntValue(assignment, "Thing.Type");
                        break;
                    case "skill1":
                        thing.Skill1 = ReadBoolValue(assignment, "Thing.Skill1");
                        break;
                    case "skill2":
                        thing.Skill2 = ReadBoolValue(assignment, "Thing.Skill2");
                        break;
                    case "skill3":
                        thing.Skill3 = ReadBoolValue(assignment, "Thing.Skill3");
                        break;
                    case "skill4":
                        thing.Skill4 = ReadBoolValue(assignment, "Thing.Skill4");
                        break;
                    case "skill5":
                        thing.Skill5 = ReadBoolValue(assignment, "Thing.Skill5");
                        break;
                    case "ambush":
                        thing.Ambush = ReadBoolValue(assignment, "Thing.Ambush");
                        break;
                    case "single":
                        thing.Single = ReadBoolValue(assignment, "Thing.Single");
                        break;
                    case "dm":
                        thing.Dm = ReadBoolValue(assignment, "Thing.Dm");
                        break;
                    case "coop":
                        thing.Coop = ReadBoolValue(assignment, "Thing.Coop");
                        break;
                    case "comment":
                        thing.Comment = ReadStringValue(assignment, "Thing.Comment");
                        break;
                    default:
                        thing.UnknownProperties.Add(new UnknownProperty(assignment.Name, assignment.ValueAsString()));
                        break;
                }
            }
            return thing;
        }

    }
}
