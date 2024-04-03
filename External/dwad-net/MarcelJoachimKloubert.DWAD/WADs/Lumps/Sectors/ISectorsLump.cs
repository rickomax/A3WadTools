using System.Collections.Generic;

namespace MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors
{
    /// <summary>
    /// Lump for SECTORS.
    /// </summary>
    public interface ISectorsLump : ILump
    {
        #region Methods (1)

        /// <summary>
        /// Enumerates all
        /// </summary>
        /// <returns></returns>
        IEnumerable<ISector> EnumerateSectors();

        #endregion Methods (1)
    }
}