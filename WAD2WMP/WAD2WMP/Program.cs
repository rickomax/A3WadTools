﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using MarcelJoachimKloubert.DWAD;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Linedefs;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Things;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Vertexes;

namespace WAD2WMP
{
    internal class Program
    {
        private const string WMPHeaderTemplate = "#  This file \"{0}\" was generated by WED v3.29\r\n#  World EDitor for 3D GameStudio by conitec GmbH 1996/1997\r\n#  creation date: 03.04.2024    time: 14:41:30";
        private const string WMPVertexHeaderTemplate = "\r\n\r\n\r\n\r\n#vertex\txpos ypos zpos index\r\n#-------------------------------\r\n";
        private const string WMPRegionHeaderTemplate = "\r\n\r\n\r\n\r\n#region\tname\tfloor_hgt\tceil_hgt\r\n#---------------------------------\r\n";
        private const string WMPWallsHeaderTemplate =  "\r\n\r\n\r\n\r\n#wall\tname vertex vertex\tregion region\toffsx offsy\tindex\r\n#------------------------------------------------------------\r\n";
        private const string WMPThingsHeaderTemplate = "\r\n\r\n\r\n\r\n#player_start\r\n#thing\r\n#actor name xpos ypos angle region index\r\n#---------------------------------------\r\n";
        private const string WMPVertexTemplate = "VERTEX\t{0}\t{1}\t0;#{2}\r\n";
        private const string WMPWallTemplate = "WALL\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6};#{7}\r\n";
        private const string WMPRegionTemplate = "REGION\t{0}\t{1}\t{2};#{3}\r\n";
        private const string WMPThingTemplate = "{0}\t{1}\t{2}\t{3}\t{4};#{5}\r\n";

        private const string WDLHeaderTemplate = "VIDEO 320x200;\r\nMAPFILE <{0}.WMP>;\r\nBIND <{1}.WDL>;\r\nNEXUS 50;\r\nCLIP_DIST 1000;\r\nLIGHT_ANGLE 1.0;\r\n";
        private const string WDLRegionTemplate = "REGION {0} {{\r\n\tCEIL_TEX {1};\r\n\tFLOOR_TEX {2};\r\n\tAMBIENT {3};\r\n}}\r\n";
        private const string WDLTextureTemplate = "TEXTURE {0} {{\r\n\tBMAPS {1};\r\n}}\r\n";
        private const string WDLBitmapTemplate = "BMAP {0} <{1}>;\r\n";
        private const string WDLWallTemplate = "WALL {0} {{\r\n\tTEXTURE {1};\r\n}}\r\n";
        private const string WDLPaletteTemplate = "PALETTE MAINPAL{{\r\n\tPALFILE <{0}>;\r\n\tRANGE 2, 254;\r\n\tFLAGS AUTORANGE;\r\n}}\r\n";

        private const string DefaultPaletteFilename = "PALETTE.PCX";
        private const string DummyBitmapFilename = "DUMMY.PCX";
        private const string DummyBitmapName = "DUMMYBMP";
        private const string DummyTextureName = "DUMMYTEX";
        private const string BorderRegionName = "BORDERRGN";

        private const float Scale = 1f / 16f;

        private static bool IsValidPath(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var filename = Path.GetFileName(path);
            var invalidChars = Path.GetInvalidFileNameChars();
            return directory != null && filename.IndexOfAny(invalidChars) < 0;
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            if (args.Length < 4)
            {
                Console.WriteLine("WAD2WMP - Usage: <Input WAD Path> <Output WMP Path> <Output WDL Path> <Force Dummy Textures Y/N>");
                Console.ReadKey();
                return;
            }
            var wadPath = args[0];
            if (!IsValidPath(wadPath) || !File.Exists(wadPath))
            {
                Console.WriteLine($"\"{wadPath}\" not found");
                Console.ReadKey();
                return;
            }
            var wmpPath = args[1];
            if (!IsValidPath(wmpPath))
            {
                Console.WriteLine($"\"{wmpPath}\" is not a valid path");
                Console.ReadKey();
                return;
            }
            var wdlPath = args[2];
            if (!IsValidPath(wdlPath))
            {
                Console.WriteLine($"\"{wdlPath}\" is not a valid path");
                Console.ReadKey();
                return;
            }
            var forceDummyTextures = args[3] == "Y" || args[3] == "y";
            var wmpFilename = Path.GetFileNameWithoutExtension(wmpPath);
            var wdlFilename = Path.GetFileNameWithoutExtension(wdlPath);
            using (var wdlStream = File.Create(wdlPath))
            {
                using (var wdlStreamWriter = new StreamWriter(wdlStream))
                {
                    wdlStreamWriter.Write(WDLHeaderTemplate, wmpFilename, wdlFilename);
                    wdlStreamWriter.Write(WDLPaletteTemplate, DefaultPaletteFilename);
                    wdlStreamWriter.Write(WDLBitmapTemplate, DummyBitmapName, DummyBitmapFilename);
                    wdlStreamWriter.Write(WDLTextureTemplate, DummyTextureName, DummyBitmapName);
                    wdlStreamWriter.Write(WDLRegionTemplate, BorderRegionName, DummyTextureName, DummyTextureName, 1f);
                    using (var wmpStream = File.Create(wmpPath))
                    {
                        using (var wmpStreamWriter = new StreamWriter(wmpStream))
                        {
                            using (var wadStream = File.OpenRead(wadPath))
                            {
                                foreach (var wadFile in WADFileFactory.FromStream(wadStream))
                                {
                                    var allSectors = wadFile
                                        .EnumerateLumps()
                                        .OfType<ISectorsLump>()
                                        .SelectMany(x => x.EnumerateSectors()).ToArray();
                                    var allVertices = wadFile
                                        .EnumerateLumps()
                                        .OfType<IVertexesLump>()
                                        .SelectMany(x => x.EnumerateVertexes()).ToArray();
                                    var allThings = wadFile
                                        .EnumerateLumps()
                                        .OfType<IThingsLump>()
                                        .SelectMany(x => x.EnumerateThings()).ToArray();
                                    var allLinedefs = wadFile
                                        .EnumerateLumps()
                                        .OfType<ILinedefsLump>()
                                        .SelectMany(x => x.EnumerateLinedefs()).ToArray();

                                    wmpStreamWriter.Write(WMPHeaderTemplate);

                                    var vertexIndex = 0;
                                    wmpStreamWriter.Write(WMPVertexHeaderTemplate);
                                    foreach (var vertex in allVertices)
                                    {
                                        wmpStreamWriter.Write(WMPVertexTemplate, vertex.X * Scale, vertex.Y * Scale, vertexIndex++);
                                    }

                                    var sectorIndex = 0;
                                    wmpStreamWriter.Write(WMPRegionHeaderTemplate);
                                    wmpStreamWriter.Write(WMPRegionTemplate, BorderRegionName, 0f, 0f, sectorIndex++);
                                    foreach (var sector in allSectors)
                                    {
                                        var regionName = $"TREGION{sectorIndex}";
                                        wmpStreamWriter.Write(WMPRegionTemplate, regionName,  sector.FloorHeight * Scale, sector.CeilingHeight * Scale, sectorIndex++);
                                        wdlStreamWriter.Write(WDLRegionTemplate, regionName, forceDummyTextures ? DummyTextureName : sector.FloorTexture, forceDummyTextures ? DummyTextureName : sector.CeilingTexture, sector.LightLevel == 0 ? 0f :sector.LightLevel / 255f);
                                    }

                                    var wallIndex = 0;
                                    wmpStreamWriter.Write(WMPWallsHeaderTemplate);
                                    foreach (var linedef in allLinedefs)
                                    {
                                        var wallName = $"TWALL{wallIndex}";
                                        var rightSide = linedef.RightSide;
                                        var leftSide = linedef.LeftSide;
                                        var vIndex1 = linedef.StartVertexIndex;
                                        var vIndex2 = linedef.EndVertexIndex;
                                        var rightSideSectorIndex = rightSide.SectorIndex + 1;
                                        var leftSideSectorIndex = leftSide == null ? 0 : leftSide.SectorIndex + 1;
                                        wmpStreamWriter.Write(WMPWallTemplate, wallName, vIndex1, vIndex2, leftSideSectorIndex, rightSideSectorIndex, rightSide.XOffset, rightSide.YOffset, wallIndex++);
                                        wdlStreamWriter.Write(WDLWallTemplate, wallName, forceDummyTextures ? DummyTextureName : rightSide.MiddleTexture); //todo: select best texture
                                    }

                                    var thingIndex = 0;
                                    wmpStreamWriter.Write(WMPThingsHeaderTemplate);
                                    foreach (var thing in allThings)
                                    {
                                        if (thing.Type == 1)
                                        {
                                            wmpStreamWriter.Write(WMPThingTemplate, "PLAYER_START", thing.X * Scale, thing.Y * Scale, thing.Angle, FindRegion(thing), thingIndex++);
                                        }
                                    }

                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string FindRegion(IThing thing)
        {
            return "0";
        }
    }
}
