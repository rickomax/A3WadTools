using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    public class PcxLoader : Loader
    {
        private int m_width;
        private int m_height;
        private int m_offset;

        private const byte c_palette = 0x0C;
        private const byte c_manufacturer = 0x0A;
        private const byte c_version = 5;

        public PcxLoader() : base()
        {
            m_offset = 0;
        }

        public override bool Load(string fileName)
        {
            if (base.Load(fileName))
            {
                if (ReadHeader())
                {
                    ConfirmReady();
                    return true;
                }
            }
            return false;
        }

        private bool ReadHeader()
        {
            int length = 128;
            if (m_offset + length <= m_bytes.Length)
            {
                if (ReadByte() == c_manufacturer && ReadByte() == c_version)
                {
                    m_offset++; //skip encoding
                    byte bpp = ReadByte();
                    int minX = ReadInt16();
                    int minY = ReadInt16();
                    int maxX = ReadInt16();
                    int maxY = ReadInt16();
                    m_offset += 4; //skip DPI
                    m_offset += 48; //skip 16 color palette
                    m_offset++; //skip reserved
                    byte planes = ReadByte();
                    m_offset += 2; //skip plane line info
                    m_offset += 2; //skip palette info
                    m_offset += 4; //skip scrolling info
                    m_offset += 54; //skip padding

                    int depth = bpp * planes;
                    int width = maxX - minX + 1;
                    int height = maxY - minY + 1;
                    if ((depth == 8) && (width > 0) && (height > 0))
                    {
                        m_width = width;
                        m_height = height;
                        return ReadBody();
                    }
                }
                Console.WriteLine("PcxLoader ERROR: Unsupported image format found.");
                return false;
            }
            else
            {
                Console.WriteLine("PcxLoader ERROR: Corrupted header data found.");
                return false;
            }
        }

        private bool ReadBody()
        {
            m_master = new Indexmap(m_width, m_height);
            int tgtIdx = 0;
            byte value;
            while (tgtIdx < m_master.Data.Length)
            {
                byte selector = ReadByte();
                if (selector > 192)
                {
                    int count = selector & 0x3F;
                    value = ReadByte();
                    if (tgtIdx + count - 1 < m_master.Data.Length)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            m_master.Data[tgtIdx++] = value;
                        }
                    }
                    else
                    {
                        Console.WriteLine("PcxLoader ERROR: Corrupted image data found.");
                        return false;
                    }
                }
                else
                {
                    m_master.Data[tgtIdx++] = selector;
                }
            }
            return ReadPalette();
        }

        private bool ReadPalette()
        {
            int length = 3 * 256;
            if (m_offset + length + 1 <= m_bytes.Length)
            {
                if (ReadByte() == c_palette)
                {
                    for (int i = length; i > 0; i -= 3)
                    {
                        byte[] rgb = new byte[3];
                        rgb[0] = ReadByte(); //R
                        rgb[1] = ReadByte(); //G
                        rgb[2] = ReadByte(); //B
                        m_palette.Add(rgb);
                    }
                    return true;
                }
            }
            Console.WriteLine("PcxLoader ERROR: Corrupted palette data found.");
            return false;
        }

        private int ReadInt16()
        {
            int value = (m_bytes[m_offset + 1] << 8) | m_bytes[m_offset];
            m_offset += 2;
            return value;
        }

        private byte ReadByte()
        {
            byte value = m_bytes[m_offset];
            m_offset++;
            return value;
        }


    }
}
