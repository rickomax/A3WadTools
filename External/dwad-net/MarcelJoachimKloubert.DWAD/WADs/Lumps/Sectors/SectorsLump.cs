using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors;
using System.Collections.Generic;
using System.Text;

namespace MarcelJoachimKloubert.DWAD.WADs
{
    partial class WADFileBase
    {
        /// <summary>
        /// 
        /// </summary>
        internal class SectorsLump : WADs.WADFileBase.UnknownLump, ISectorsLump
        {
            public IEnumerable<ISector> EnumerateSectors()
            {
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
                        var floorHeight = ToInt16(buffer).Value;

                        buffer = new byte[2];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var ceilingHeight = ToInt16(buffer).Value;

                        buffer = new byte[8];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var floorTexture = Encoding.ASCII.GetString(buffer).ToUpper().TrimEnd('\0'); 

                        buffer = new byte[8];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var ceilingTexture = Encoding.ASCII.GetString(buffer).ToUpper().TrimEnd('\0');

                        buffer = new byte[2];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var lightLevel = ToInt16(buffer).Value;

                        buffer = new byte[2];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var specialType = ToInt16(buffer).Value;

                        buffer = new byte[2];
                        if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                        {
                            continue;
                        }
                        var tagNumber = ToInt16(buffer).Value;

                        yield return new Sector()
                        {
                            Lump = this,
                            FloorHeight = floorHeight,
                            CeilingHeight = ceilingHeight,
                            FloorTexture = floorTexture,
                            CeilingTexture = ceilingTexture,
                            LightLevel = lightLevel,
                            SpecialType = specialType,
                            TagNumber = tagNumber
                        };

                        hasNext = true;
                    }
                    while (hasNext);
                }
            }
        }
    }
}