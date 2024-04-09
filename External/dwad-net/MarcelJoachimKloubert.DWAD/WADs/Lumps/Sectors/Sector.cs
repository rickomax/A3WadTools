namespace MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors
{
    /// <summary>
    /// 
    /// </summary>
    internal class Sector : ISector
    {  
        /// <summary>
        /// 
        /// </summary>
        public short FloorHeight { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public short CeilingHeight { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string FloorTexture { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string CeilingTexture { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public short LightLevel { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public short SpecialType { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public short TagNumber { get; internal set; }

        internal ISectorsLump Lump { get; set; }

        ISectorsLump ISector.Lump => this.Lump;
    }
}
