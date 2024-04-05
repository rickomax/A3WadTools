using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Loader loader;
            loader = new LbmLoader();
            if (loader.Load(@"..\..\bitmaps\michels.lbm"))
            {
                Printer.PrintImage(loader.Master, loader.Palette, 50, 50);
                int tileIdx;
                tileIdx = loader.GetTile(520, 72, 110, 120);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 670, 50);
                tileIdx = loader.GetTile(64, 192, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 800, 50);
                tileIdx = loader.GetTile(64, 192, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 870, 50);
            }
            loader = new PcxLoader();
            if (loader.Load(@"..\..\bitmaps\floortex.pcx"))
            {
                Printer.PrintImage(loader.Master, loader.Palette, 50, 320);
                int tileIdx;
                tileIdx = loader.GetTile(0, 0, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 500, 320);
                tileIdx = loader.GetTile(64, 64, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 570, 320);
                tileIdx = loader.GetTile(128, 0, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 640, 320);
            }
        }
    }
}
