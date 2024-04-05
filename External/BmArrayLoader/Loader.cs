using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    public abstract class Loader
    {
        protected List<byte[]> m_palette;
        protected byte[] m_bytes;
        protected Indexmap m_master;

        private bool m_loaded;
        private bool m_ready;
        private List<Indexmap> m_tiles;

        public Indexmap Master { get => m_master; }
        public List<Indexmap> Tiles { get => m_tiles; }
        public List<byte[]> Palette { get => m_palette; }

        public Loader()
        {
            m_palette = new List<byte[]>(256);
            m_tiles = new List<Indexmap>();
            m_loaded = false;
            m_ready = false;
        }

        public virtual bool Load(string fileName)
        {
            try
            {
                m_bytes = File.ReadAllBytes(fileName);
                m_loaded = true;
            }
            catch
            {
                m_loaded = false;
                Console.WriteLine("Loader ERROR: Loading " + fileName + " failed!");
            }
            return m_loaded;
        }

        public int GetTile(int offsetX, int offsetY, int width, int height)
        {
            //Indexmap already in list?
            int idx = m_tiles.FindIndex(x => x.Equals(offsetX, offsetY, width, height));
            if (idx == -1)
            {
                //derive newindex map from master
                int sizeX = offsetX + width;
                int sizeY = offsetY + height;

                if (m_ready &&
                    sizeX > 0 && sizeY > 0 &&
                    width > 0 && height > 0 &&
                    sizeX * sizeY < m_master.Data.Length)
                {
                    Indexmap indexmap = new Indexmap(offsetX, offsetY, width, height);
                    int srcIdx = 0;
                    int tgtIdx = 0;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            srcIdx = m_master.Width * (offsetY + y) + offsetX + x;
                            indexmap.Data[tgtIdx] = m_master.Data[srcIdx];
                            tgtIdx++;
                        }
                    }
                    m_tiles.Add(indexmap);
                    idx =  m_tiles.Count - 1;
                }
            }
            return idx;
        }

        protected void ConfirmReady()
        {
            m_tiles.Add(m_master);
            m_ready = true;
        }
    }
}
