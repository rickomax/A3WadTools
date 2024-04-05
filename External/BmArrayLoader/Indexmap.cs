using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    public class Indexmap
    {
        private readonly int m_offsetX;
        private readonly int m_offsetY;
        private readonly int m_width;
        private readonly int m_height;
        private readonly byte[] m_data;

        public Indexmap(int width, int height) : this(0, 0, width, height) { }
        public Indexmap(int offsetX, int offsetY, int width, int height)
        {
            m_offsetX = offsetX;
            m_offsetY = offsetY;
            m_width = width;
            m_height = height;
            m_data = new byte[width * height];
        }

        public int Width { get => m_width; }
        public int Height { get => m_height; }
        public byte[] Data { get => m_data; }

        public bool Equals(int offsetX, int offsetY, int width, int height)
        {
            if (offsetX == m_offsetX && offsetY == m_offsetY &&
                width == m_width && height == m_height)
                return true;
            else
                return false;
        }
    }
}
