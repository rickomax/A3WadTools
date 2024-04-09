using MarcelJoachimKloubert.DWAD.WADs.Lumps.Linedefs;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sidedefs;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Things;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Vertexes;

namespace WAD2WMP
{
    public class UDMFVertex : IVertex
    {
        public IVertexesLump Lump { get; }
        public short X { get; set; }
        public short Y { get; set; }
    }
    public class UDMFSector : ISector
    {
        public short FloorHeight { get; set; }
        public short CeilingHeight { get; set; }
        public string FloorTexture { get; set; }
        public string CeilingTexture { get; set; }
        public short LightLevel { get; set; }
        public short SpecialType { get; set; }
        public short TagNumber { get; }
        public ISectorsLump Lump { get; }
    }

    public class UDMFSidedef : ISidedef
    {
        public short XOffset { get; set; }
        public short YOffset { get; set; }
        public string UpperTexture { get; set; }
        public string LowerTexture { get; set; }
        public string MiddleTexture { get; set; }
        public ISector Sector { get; set; }
        public ISidedefsLump Lump { get; }
        public short SectorIndex { get; set; }
    }

    public class UDMFLinedef : ILinedef
    {
        public IVertex End { get; set; }
        public double Length { get; }
        public ILinedefsLump Lump { get; }
        public IVertex Start { get; set; }
        public ISidedef RightSide { get; set; }
        public ISidedef LeftSide { get; set; }
        public short Flags { get; }
        public short SectorTag { get; }
        public short SpecialType { get; set; }
        public short RightSideIndex { get; set; }
        public short LeftSideIndex { get; set; }
        public short StartVertexIndex { get; set; }
        public short EndVertexIndex { get; set; }
    }
    
    public class UDMFThing : IThing
    {
        public short Angle { get; set; }
        public short Flags { get; }
        public int Index { get; set; }
        public IThingsLump Lump { get; }
        public short Type { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
    }
}