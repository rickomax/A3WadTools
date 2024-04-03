namespace MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors
{
    /// <summary>
    /// 
    /// </summary>
    internal class Sector : ISector
    {
        public short FloorHeight { get; internal set; }

        public short CeilingHeight { get; internal set; }

        public string FloorTexture { get; internal set; }

        public string CeilingTexture { get; internal set; }

        public short LightLevel { get; internal set; }

        public short SpecialType { get; internal set; }

        public short TagNumber { get; internal set; }

        internal ISectorsLump Lump { get; set; }

        ISectorsLump ISector.Lump => this.Lump;
    }
}
