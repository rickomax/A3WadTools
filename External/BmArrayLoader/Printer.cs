using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace BmArrayLoader
{
    public static class Printer
    {
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow", SetLastError = true)]
        private static extern IntPtr GetConsoleHandle();

        static public void PrintImage(Indexmap master, List<byte[]> palette, int offsetX, int offsetY)
        {
            Bitmap bitmap = new Bitmap(master.Width, master.Height);
            for (int i = 0; i < master.Data.Length; i++)
            {
                int index = master.Data[i];
                Color color = Color.FromArgb(palette[index][0], palette[index][1], palette[index][2]);
                bitmap.SetPixel(i % master.Width, i / master.Width, color);
            }
            var handler = GetConsoleHandle();
            using (var graphics = Graphics.FromHwnd(handler))
            {
                using (bitmap)
                {
                    graphics.DrawImage(bitmap, offsetX, offsetY, bitmap.Width, bitmap.Height);
                }
            }
        }
    }
}
