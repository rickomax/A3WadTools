﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using MarcelJoachimKloubert.DWAD;
using MarcelJoachimKloubert.DWAD.WADs.Lumps;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Linedefs;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sectors;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sidedefs;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Things;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Vertexes;
using WADCommon;

namespace WAD2WMP
{
    internal class Program
    {
        private const string WMPHeaderTemplate = "#  This file \"{0}\" was generated by WED v3.29\r\n#  World EDitor for 3D GameStudio by conitec GmbH 1996/1997\r\n#  creation date: 03.04.2024    time: 14:41:30";
        private const string WMPVertexHeaderTemplate = "\r\n\r\n\r\n\r\n#vertex\txpos ypos zpos index\r\n#-------------------------------\r\n";
        private const string WMPRegionHeaderTemplate = "\r\n\r\n\r\n\r\n#region\tname\tfloor_hgt\tceil_hgt\r\n#---------------------------------\r\n";
        private const string WMPWallsHeaderTemplate = "\r\n\r\n\r\n\r\n#wall\tname vertex vertex\tregion region\toffsx offsy\tindex\r\n#------------------------------------------------------------\r\n";
        private const string WMPThingsHeaderTemplate = "\r\n\r\n\r\n\r\n#player_start\r\n#thing\r\n#actor name xpos ypos angle region index\r\n#---------------------------------------\r\n";
        private const string WMPVertexTemplate = "VERTEX\t{0}\t{1}\t0;#{2}\r\n";
        private const string WMPWallTemplate = "WALL\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6};#{7}\r\n";
        private const string WMPRegionTemplate = "REGION\t{0}\t{1}\t{2};#{3}\r\n";
        private const string WMPThingTemplate = "{0}\t{1}\t{2}\t{3}\t{4}\t{5};#{6}\r\n";

        private const string WDLHeaderTemplate = "VIDEO 320x200;\r\nMAPFILE <{0}.WMP>;\r\nBIND <{1}.WDL>;\r\nNEXUS 50;\r\nCLIP_DIST 1000;\r\nLIGHT_ANGLE 1.0;\r\n";
        private const string WDLRegionTemplate = "REGION {0} {{\r\n\tCEIL_TEX {1};\r\n\tFLOOR_TEX {2};\r\n\tAMBIENT {3};\r\n}}\r\n";
        private const string WDLTextureTemplate = "TEXTURE {0} {{\r\n\tSCALE_XY {2}, {3};\r\n\tBMAPS {1};\r\n}}\r\n";
        private const string WDLBitmapTemplate = "BMAP {0} <{1}>;\r\n";
        private const string WDLWallTemplate = "WALL {0} {{\r\n\tTEXTURE {1};\r\n\tFLAGS PORTCULLIS;\r\n}}\r\n";
        private const string WDLPaletteTemplate = "PALETTE MAINPAL{{\r\n\tPALFILE <{0}>;\r\n\tRANGE 2, 254;\r\n\tFLAGS AUTORANGE;\r\n}}\r\n";

        private const string DefaultPaletteFilename = "PALETTE.PCX";
        private const string BorderRegionName = "BORDERRGN";

        private const string WallTextureSuffix = "WALLTEX";
        private const string RegionTextureSuffix = "REGTEX";

        private const string DummyBitmapFilename = "DUMMY.PCX";
        private const string DummyBitmapName = "DUMMYBMP";
        private const string DummyTextureName = "DUMMYWALLTEX";

        private static readonly char[] Separators = new char[] { ',' };

        private struct AcknexThing
        {
            public string Type;
            public string Name;

            public AcknexThing(string type, string name)
            {
                Type = type;
                Name = name;
            }
        }

        private class AcknexRegion
        {
            public AcknexRegion Parent;
            public bool HasChildren;
            public int Index;
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            if (args.Length < 4)
            {
                Console.WriteLine("WAD2WMP - Usage: <Input WAD Path> <Output WMP Path> <Output WDL Path> <Add WDL Header: Y/N> <Optional: Force Dummy Textures Y/N>");
                Console.ReadKey();
                return;
            }
            var wadPath = args[0];
            if (!Common.IsValidPath(wadPath) || !File.Exists(wadPath))
            {
                Console.WriteLine($"\"{wadPath}\" not found");
                Console.ReadKey();
                return;
            }
            var wmpPath = args[1];
            if (!Common.IsValidPath(wmpPath))
            {
                Console.WriteLine($"\"{wmpPath}\" is not a valid path");
                Console.ReadKey();
                return;
            }
            var wdlPath = args[2];
            if (!Common.IsValidPath(wdlPath))
            {
                Console.WriteLine($"\"{wdlPath}\" is not a valid path");
                Console.ReadKey();
                return;
            }
            var addWdlHeader = args[3] == "Y" || args[3] == "y";
            var forceDummyTextures = args.Length >= 5 && args[4] == "Y" || args[4] == "y";
            var wmpFilename = Path.GetFileNameWithoutExtension(wmpPath);
            var wdlFilename = Path.GetFileNameWithoutExtension(wdlPath);
            var wdlDirectory = Path.GetDirectoryName(wdlPath);
            using (var wdlStream = File.Create(wdlPath))
            {
                using (var wdlStreamWriter = new StreamWriter(wdlStream))
                {
                    using (var wmpStream = File.Create(wmpPath))
                    {
                        using (var wmpStreamWriter = new StreamWriter(wmpStream))
                        {
                            using (var wadStream = File.OpenRead(wadPath))
                            {
                                foreach (var wadFile in WADFileFactory.FromStream(wadStream))
                                {
                                    if (addWdlHeader)
                                    {
                                        wdlStreamWriter.Write(WDLHeaderTemplate, wmpFilename, wdlFilename);
                                    }

                                    wdlStreamWriter.Write(WDLBitmapTemplate, DummyBitmapName, DummyBitmapFilename);
                                    wdlStreamWriter.Write(WDLTextureTemplate, DummyTextureName, DummyBitmapName, Common.AckScale, Common.AckScale);

                                    var lumps = wadFile.EnumerateLumps().ToArray();

                                    var processedTextures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                                    Color[] palette = null;
                                    var playPal = lumps.FirstOrDefault(x => x.Name == "PLAYPAL");
                                    if (playPal != null)
                                    {
                                        var palData = Common.ReadFully(playPal.GetStream());
                                        palette = new Color[256];
                                        for (int i = 0, offset = 0; i < 256; i++, offset += 3)
                                        {
                                            palette[i] = new Color(palData[offset], palData[offset + 1], palData[offset + 2]);
                                        }
                                        var pixelData = new byte[256];
                                        for (var i = 0; i < 256; i++)
                                        {
                                            pixelData[i] = (byte)i;
                                        }
                                        ExtractTexture(playPal, delegate (BinaryWriter textureWriter)
                                        {
                                            PCXWriter.WritePCX(pixelData, 256, 1, palette, textureWriter);
                                        }, wdlStreamWriter, processedTextures, wdlDirectory, false);
                                        wdlStreamWriter.Write(WDLPaletteTemplate, "PLAYPAL.PCX");
                                    }
                                    else
                                    {
                                        wdlStreamWriter.Write(WDLPaletteTemplate, DefaultPaletteFilename);
                                    }

                                    var insideTextures = false;
                                    var insideFlats = false;
                                    var insidePatches = false;
                                    var availableTextures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                                    var dummyTextures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                                    var ackTable = new Dictionary<short, AcknexThing>();

                                    foreach (var lump in lumps)
                                    {
                                        if (!forceDummyTextures)
                                        {
                                            switch (lump.Name)
                                            {
                                                case "ACKTABLE": //todo
                                                    {
                                                        var streamReader = new StreamReader(lump.GetStream());
                                                        var acktableContent = streamReader.ReadToEnd();
                                                        var lines = acktableContent.Split('\n', '\r');
                                                        foreach (var line in lines)
                                                        {
                                                            if (string.IsNullOrWhiteSpace(line))
                                                            {
                                                                continue;
                                                            }
                                                            var data = line.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
                                                            ackTable.Add(short.Parse(data[0]), new AcknexThing(data[1], data[2]));
                                                        }
                                                        continue;
                                                    }
                                                case "TX_START":
                                                    insideTextures = true;
                                                    continue;
                                                case "TX_END":
                                                    insideTextures = false;
                                                    continue;
                                                case "P1_START":
                                                case "P2_START":
                                                    insidePatches = true;
                                                    continue;
                                                case "P1_END":
                                                case "P2_END":
                                                    insidePatches = false;
                                                    continue;
                                                case "F1_START":
                                                case "F2_START":
                                                    insideFlats = true;
                                                    continue;
                                                case "F1_END":
                                                case "F2_END":
                                                    insideFlats = false;
                                                    continue;
                                            }
                                            var width = 0;
                                            var height = 0;
                                            byte[] pixelData = null;
                                            var textureData = Common.ReadFully(lump.GetStream());
                                            if (availableTextures.Contains(lump.Name))
                                            {
                                                continue;
                                            }
                                            if (insideFlats)
                                            {
                                                if (palette == null)
                                                {
                                                    Console.WriteLine($"Skipping image {lump.Name} because a palette has not been found");
                                                    continue;
                                                }
                                                if (textureData.Length != 4096)
                                                {
                                                    Console.WriteLine($"Skipping image {lump.Name} because it isn't a 64x64 flat");
                                                    continue;
                                                }
                                                width = 64;
                                                height = 64;
                                                pixelData = textureData;
                                            }
                                            else if (insidePatches || insideTextures)
                                            {
                                                if (palette == null)
                                                {
                                                    Console.WriteLine($"Skipping image {lump.Name} because a palette has not been found");
                                                    continue;
                                                }
                                                if (textureData.Length >= 2 && textureData[0] == 10 && textureData[1] == 5) //PCX
                                                {
                                                    availableTextures.Add(lump.Name);
                                                    ExtractTexture(lump, delegate (BinaryWriter textureWriter)
                                                    {
                                                        textureWriter.Write(textureData);
                                                    }, wdlStreamWriter, processedTextures, wdlDirectory);
                                                }
                                                else
                                                {
                                                    width = Common.GetInt16Le(textureData, 0);
                                                    height = Common.GetInt16Le(textureData, 2);
                                                    pixelData = new byte[width * height];
                                                    for (var i = 0; i < pixelData.Length; i++)
                                                    {
                                                        pixelData[i] = 255;
                                                    }

                                                    for (var column = 0; column < width; column++)
                                                    {
                                                        var pointer = Common.GetInt32Le(textureData, (column * 4) + 8);
                                                        do
                                                        {
                                                            int postHeight;
                                                            var row = textureData[pointer];
                                                            if (row != 255 && (postHeight = textureData[++pointer]) != 255)
                                                            {
                                                                pointer++;
                                                                for (var i = 0; i < postHeight; i++)
                                                                {
                                                                    if (row + i < height && pointer < textureData.Length - 1)
                                                                    {
                                                                        pixelData[((row + i) * width) + column] = textureData[++pointer];
                                                                    }
                                                                }

                                                                pointer++;
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        } while (pointer < textureData.Length - 1 && textureData[++pointer] != 255);
                                                    }
                                                }
                                            }
                                            if (pixelData != null)
                                            {
                                                availableTextures.Add(lump.Name);
                                                ExtractTexture(lump, delegate (BinaryWriter textureWriter)
                                                {
                                                    PCXWriter.WritePCX(pixelData, width, height, palette, textureWriter);
                                                }, wdlStreamWriter, processedTextures, wdlDirectory);
                                            }
                                        }
                                    }

                                    var allSectors = lumps
                                        .OfType<ISectorsLump>()
                                        .SelectMany(x => x.EnumerateSectors()).ToArray();
                                    var allVertices = lumps
                                        .OfType<IVertexesLump>()
                                        .SelectMany(x => x.EnumerateVertexes()).ToArray();
                                    var allThings = lumps
                                        .OfType<IThingsLump>()
                                        .SelectMany(x => x.EnumerateThings()).ToArray();
                                    var allLinedefs = lumps
                                        .OfType<ILinedefsLump>()
                                        .SelectMany(x => x.EnumerateLinedefs()).ToArray();

                                    var sectorScore = new int[allSectors.Length];
                                    BuildSectorScore(sectorScore, allSectors, allLinedefs);

                                    wmpStreamWriter.Write(WMPHeaderTemplate);

                                    var vertexIndex = 0;
                                    wmpStreamWriter.Write(WMPVertexHeaderTemplate);
                                    foreach (var vertex in allVertices)
                                    {
                                        wmpStreamWriter.Write(WMPVertexTemplate, vertex.X * Common.Scale, vertex.Y * Common.Scale, vertexIndex++);
                                    }

                                    var sectorIndex = 0;
                                    wmpStreamWriter.Write(WMPRegionHeaderTemplate);
                                    wmpStreamWriter.Write(WMPRegionTemplate, BorderRegionName, 0f, 0f, sectorIndex++);
                                    wdlStreamWriter.Write(WDLRegionTemplate, BorderRegionName, DummyTextureName, DummyTextureName, 1f);
                                    foreach (var sector in allSectors)
                                    {
                                        var regionName = $"TREGION{sectorIndex}";
                                        wmpStreamWriter.Write(WMPRegionTemplate, regionName, sector.FloorHeight * Common.Scale, sector.CeilingHeight * Common.Scale, sectorIndex++);
                                        wdlStreamWriter.Write(WDLRegionTemplate, regionName, forceDummyTextures ? DummyTextureName : ProcessTexture(wdlStreamWriter, availableTextures, dummyTextures, sector.CeilingTexture, true), forceDummyTextures ? DummyTextureName : ProcessTexture(wdlStreamWriter, availableTextures, dummyTextures, sector.FloorTexture, true), sector.LightLevel == 0 ? 0f : sector.LightLevel / 255f);
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
                                        wmpStreamWriter.Write(WMPWallTemplate, wallName, vIndex1, vIndex2, leftSideSectorIndex, rightSideSectorIndex, rightSide.XOffset * Common.Scale, rightSide.YOffset != 0 ? (rightSide.YOffset - 5f) * Common.Scale : 0f, wallIndex++);
                                        wdlStreamWriter.Write(WDLWallTemplate, wallName, forceDummyTextures ? DummyTextureName : ProcessTexture(wdlStreamWriter, availableTextures, dummyTextures, SelectTexture(rightSide.LowerTexture, rightSide.UpperTexture, rightSide.MiddleTexture)));
                                    }

                                    var thingIndex = 0;
                                    wmpStreamWriter.Write(WMPThingsHeaderTemplate);
                                    foreach (var thing in allThings)
                                    {
                                        if (thing.Type == 1)
                                        {
                                            wmpStreamWriter.Write(WMPThingTemplate, "PLAYER_START", "", thing.X * Common.Scale, thing.Y * Common.Scale, thing.Angle, FindRegion(thing, sectorScore, allSectors, allLinedefs), thingIndex++);
                                        }
                                        else if (ackTable.TryGetValue(thing.Type, out var acknexThing))
                                        {
                                            wmpStreamWriter.Write(WMPThingTemplate, acknexThing.Type, acknexThing.Name, thing.X * Common.Scale, thing.Y * Common.Scale, thing.Angle, FindRegion(thing, sectorScore, allSectors, allLinedefs), thingIndex++);
                                        }
                                    }
                                    Console.WriteLine("Finished exporting. Press any key to exit");
                                    Console.ReadKey();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string ProcessTexture(StreamWriter wdlStreamWriter, HashSet<string> availableTextures, HashSet<string> dummyTextures, string texture, bool isRegion = false)
        {
            if (texture == "-")
            {
                return DummyTextureName;
            }
            if (!availableTextures.Contains(texture) && !dummyTextures.Contains(texture))
            {
                dummyTextures.Add(texture);
                Console.WriteLine($"Could not find texture {texture}");
                WriteTextureAndBitmap(texture, wdlStreamWriter, DummyBitmapFilename);
            }

            return $"{texture}{(isRegion ? RegionTextureSuffix : WallTextureSuffix)}";
        }

        private static void ExtractTexture(ILump lump, Action<BinaryWriter> writingDelegate, StreamWriter wdlStreamWriter, HashSet<string> processedTextures, string wdlDirectory, bool writeWDL = true)
        {
            var textureFilename = $"{lump.Name}.pcx";
            var texturePath = $"{wdlDirectory}\\{textureFilename}";
            if (Common.IsValidPath(texturePath))
            {
                using (var textureStream = new BinaryWriter(File.Create(texturePath)))
                {
                    writingDelegate(textureStream);
                }
            }
            if (!processedTextures.Add(lump.Name))
            {
                Console.WriteLine($"Duplicated texture definition:{lump.Name}");
                return;
            }
            if (writeWDL)
            {
                WriteTextureAndBitmap(lump.Name, wdlStreamWriter, textureFilename);
            }
        }

        private static void WriteTextureAndBitmap(string name, StreamWriter wdlStreamWriter, string textureFilename)
        {
            var bitmapName = $"{name}BMP";
            var wallTextureName = $"{name}{WallTextureSuffix}";
            var regionTextureName = $"{name}{RegionTextureSuffix}";
            wdlStreamWriter.Write(WDLBitmapTemplate, bitmapName, textureFilename);
            wdlStreamWriter.Write(WDLTextureTemplate, wallTextureName, bitmapName, -Common.AckScale, Common.AckScale);
            wdlStreamWriter.Write(WDLTextureTemplate, regionTextureName, bitmapName, -Common.AckScale, -Common.AckScale);
        }

        private static string SelectTexture(string a, string b, string c)
        {
            if (a != "-")
            {
                return a;
            }
            if (b != "-")
            {
                return b;
            }
            return c;
        }

        private static void BuildSectorScore(int[] sectorScore, ISector[] allSectors, ILinedef[] allLinedefs)
        {
            for (var outerSectorIndex = 0; outerSectorIndex < allSectors.Length; outerSectorIndex++)
            {
                for (var innerSectorIndex = 0; innerSectorIndex < allSectors.Length; innerSectorIndex++)
                {
                    if (innerSectorIndex == outerSectorIndex)
                    {
                        continue;
                    }
                    var index = innerSectorIndex;
                    var innerSectorLines = allLinedefs.Where(x => x.RightSide.SectorIndex == index);
                    foreach (var innerSectorLine in innerSectorLines)
                    {
                        var p1 = new Common.Point(innerSectorLine.Start.X, innerSectorLine.Start.Y);
                        var p2 = new Common.Point(innerSectorLine.End.X, innerSectorLine.End.Y);
                        if (IsInsideRegion(allLinedefs, p1, outerSectorIndex) || IsInsideRegion(allLinedefs, p2, outerSectorIndex))
                        {
                            sectorScore[innerSectorIndex]++;
                            break;
                        }
                    }
                }
            }
        }

        private static string FindRegion(IThing thing, int[] sectorScore, ISector[] allSectors, ILinedef[] allLinedefs)
        {
            var thingPoint = new Common.Point(thing.X, thing.Y);
            if (FindInnerRegion(allSectors, sectorScore, allLinedefs, thingPoint, out var regionIndex))
            {
                return (regionIndex + 1).ToString(CultureInfo.InvariantCulture);
            }
            return "0";
        }

        private static bool FindInnerRegion(ISector[] allSectors, int[] sectorScore, ILinedef[] allLinedefs, Common.Point thingPoint, out int outIndex)
        {
            var indices = Enumerable.Range(0, sectorScore.Length).ToList();
            indices.Sort((a, b) => sectorScore[b].CompareTo(sectorScore[a]));
            foreach (var index in indices)
            {
                if (IsInsideRegion(allLinedefs, thingPoint, index))
                {
                    outIndex = index;
                    return true;
                }
            }
            outIndex = -1;
            return false;
        }

        private static bool IsInsideRegion(ILinedef[] allLinedefs, Common.Point point, int sectorIndex)
        {
            var sectorLines = allLinedefs.Where(x => x.RightSide.SectorIndex == sectorIndex);
            foreach (var sectorLine in sectorLines)
            {
                var p1 = new Common.Point(sectorLine.Start.X, sectorLine.Start.Y);
                var p2 = new Common.Point(sectorLine.End.X, sectorLine.End.Y);
                if (Common.FindSide(p1, p2, point) > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
