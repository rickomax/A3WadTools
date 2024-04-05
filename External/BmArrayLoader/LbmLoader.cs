using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    public class LbmLoader : Loader
    {
        private int m_width;
        private int m_height;
        private int m_compressed;
        private int m_offset;

        public LbmLoader() : base()
        {
            m_offset = 0;
        }

        public override bool Load(string fileName)
        {
            if (base.Load(fileName))
            {
                if (ReadChunks())
                {
                    ConfirmReady();
                    return true;
                }
            }
            return false;
        }

        private bool ReadChunks()
        {
            bool status = false;
            bool first = true;
            if (m_bytes.Length > 8)
            {
                do
                {
                    int chunkId = ReadInt32();
                    if (first && chunkId != (int)LbmChunk.FORM) //first chunk must be FORM
                    {
                        Console.WriteLine("LbmLoader ERROR: Unsupported image format found.");
                        return false;
                    }
                    first = false;
                    int length = ReadInt32();
                    int padding = length % 2;
                    if (m_offset + length + padding <= m_bytes.Length)
                    {
                        switch (chunkId)
                        {
                            case (int)LbmChunk.FORM:
                                status = ReadForm(length);
                                //FORM is container for all other chunks, therefore don't apply padding here - it is at eof
                                break;

                            case (int)LbmChunk.BMHD:
                                status = ReadHeader(length);
                                m_offset += padding;
                                break;

                            case (int)LbmChunk.CMAP:
                                status = ReadColormap(length);
                                m_offset += padding;
                                break;

                            case (int)LbmChunk.BODY:
                                status = ReadBody(length);
                                m_offset += padding;
                                //BODY is optional - file might only contain color palette
                                break;

                            default:
                                status = SkipChunk(length);
                                m_offset += padding;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("LbmLoader ERROR: Truncated chunk found.");
                        status = false;
                    }
                } while (status && (m_bytes.Length - m_offset > 4)); //make sure padding byte does not start another iteration
            }

            return status;
        }

        private bool ReadForm(int length)
        {
            int type = ReadInt32();
            switch (type)
            {
                case (int)LbmType.PBM:
                    return true;

                case (int)LbmType.ILBM:
                    Console.WriteLine("LbmLoader ERROR: ILBM format not supported.");
                    return false;

                default:
                    Console.WriteLine("LbmLoader ERROR: Unknown format identifier found.");
                    return false;
            }
        }

        private bool ReadHeader(int length)
        {
            m_width = ReadInt16();
            m_height = ReadInt16();
            m_offset += 4; //skip stuff
            int depth = ReadByte();
            m_offset++; //skip stuff
            m_compressed = ReadByte();
            m_offset += 9; //skip stuff
            if (depth == 0 || depth == 8) //only palette or indexed color supported
            {
                return true;
            }
            else
            {
                Console.WriteLine("LbmLoader ERROR: Unsupported color depth found.");
                return false;
            }
        }

        private bool ReadBody(int length)
        {
            m_master = new Indexmap(m_width, m_height);
            switch (m_compressed)
            {
                case (int)LbmCompression.None:
                    return ReadUncompressed(length);

                case (int)LbmCompression.ByteRun1:
                    return ReadCompressed(length);

                default:
                    Console.WriteLine("LbmLoader ERROR: Unknown compression found.");
                    return false;
            }
        }

        private bool ReadUncompressed(int length)
        {
            //LBM applies padding to get lines with n*16 bit length
            //parser only supports depth of 8 bit (=1 byte), therefore just pad odd width with 1 byte
            int linePadding = m_width % 2;
//            if ((m_width % 16) != 0)
//                linePadding = 16 - (m_width % 16); //TODO this is wrong - BIT not byte, index buffer will always have 8 bit -> check even/odd width only
            int lineIdx;
            int tgtIdx = 0;

            for (int i = 0; i < length; i++)
            {
                if (tgtIdx < m_master.Data.Length - 1)
                {
                    lineIdx = i % (m_width + linePadding);
                    if (lineIdx < m_width)
                    {
                        m_master.Data[tgtIdx++] = ReadByte();
                    }
                    else
                        m_offset++; //ignore padding
                }
                else
                {
                    Console.WriteLine("LbmLoader ERROR: Corrupted body data found.");
                    return false;
                }
            }

            return true;
        }

        private bool ReadCompressed(int length)
        {
            byte selector;
            byte value;
            int count;
            int tgtIdx = 0;
            for (int i = 0; i < length; i++)
            {
                selector = ReadByte();
                if (selector < 128)
                {
                    count = selector + 1;
                    if ((tgtIdx < m_master.Data.Length - count) && (i + 1 < length - count))
                    {
                        for (int j = 0; j < count; j++)
                            m_master.Data[tgtIdx++] = ReadByte();
                        i += count; //update iterator
                    }
                    else
                    {
                        Console.WriteLine("LbmLoader ERROR: Corrupted body data found.");
                        return false;
                    }
                }
                else if (selector > 128)
                {
                    count = 257 - selector;
                    if (tgtIdx < m_master.Data.Length - 1)
                    {
                        value = ReadByte();
                        for (int j = 0; j < count; j++)
                            m_master.Data[tgtIdx++] = value;
                        i++; //update iterator
                    }
                    else
                    {
                        Console.WriteLine("LbmLoader ERROR: Corrupted body data found.");
                        return false;
                    }
                }
                else //ignore value 128
                {
                    m_offset++;
                }
            }
            return true;
        }

        private bool ReadColormap(int length)
        {
            if (length % 3 == 0)
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
            else //truncated chunk
                return false;
        }

        private bool SkipChunk(int length)
        {
            m_offset += length; //just ignore contents
            return true;
        }

        private int ReadInt32()
        {
            int value = (m_bytes[m_offset] << 24) | (m_bytes[m_offset + 1] << 16) | (m_bytes[m_offset + 2] << 8) | m_bytes[m_offset + 3];
            m_offset += 4;
            return value;
        }

        private int ReadInt16()
        {
            int value = (m_bytes[m_offset] << 8) | m_bytes[m_offset + 1];
            m_offset += 2;
            return value;
        }

        private byte ReadByte()
        {
            byte value = m_bytes[m_offset];
            m_offset++;
            return value;
        }

        private enum LbmType : int
        {
            ILBM = 0x494C424D,
            PBM = 0x50424D20,
        }

        private enum LbmCompression : int
        {
            None = 0,
            ByteRun1 = 1,
        }

        private enum LbmChunk : int
        {
            FORM = 0x464F524D,
            BMHD = 0x424D4844,
            BODY = 0x424F4459,
            CMAP = 0x434D4150,
        }
    }
}
