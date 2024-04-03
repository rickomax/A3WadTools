using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sidedefs;

namespace MarcelJoachimKloubert.DWAD.WADs
{
    partial class WADFileBase
    {
        /// <summary>
        /// 
        /// </summary>
        internal class SidedefsLump : WADs.WADFileBase.UnknownLump, ISidedefsLump
        {
            public IEnumerable<ISidedef> EnumerateSidedefs()
            {
                var allSectors = this.File
                    .EnumerateLumps()
                    .OfType<ISectorsLump>()
                    .SelectMany(x => x.EnumerateSectors())
                    .ToArray();

                using (var stream = this.GetStream())
                {
                    bool hasNext;

                    do
                    {
                        hasNext = false;

                        byte[] buffer;

                        buffer = new byte[2];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var xOffset = ToInt16(buffer).Value;

                        buffer = new byte[2];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var yOffset = ToInt16(buffer).Value;

                        buffer = new byte[8];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var upperTexture = Encoding.ASCII.GetString(buffer).ToUpper().TrimEnd('\0');

                        buffer = new byte[8];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var lowerTexture = Encoding.ASCII.GetString(buffer).ToUpper().TrimEnd('\0');


                        buffer = new byte[8];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var middleTexture = Encoding.ASCII.GetString(buffer).ToUpper().TrimEnd('\0');

                        buffer = new byte[2];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var sectorIndex = ToInt16(buffer).Value;

                        yield return new Sidedef()
                        {
                            Lump = this,
                            XOffset =  xOffset,
                            YOffset = yOffset,
                            LowerTexture = lowerTexture,
                            UpperTexture = upperTexture,
                            MiddleTexture = middleTexture,
                            Sector = allSectors[sectorIndex],
                            SectorIndex = sectorIndex
                        };

                        hasNext = true;
                    }
                    while (hasNext);
                }
            }
        }
    }
}