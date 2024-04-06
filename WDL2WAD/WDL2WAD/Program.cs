using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BmArrayLoader;
using MarcelJoachimKloubert.DWAD;
using MarcelJoachimKloubert.DWAD.WADs.Lumps;
using VCCCompiler;
using WADCommon;

namespace WDL2WAD
{
    internal class Program
    {

        private const string DecorateActorTemplate = "Actor {0} : DoomImp {1}\r\n{{\r\n  Radius {2}\r\n  States\r\n  {{\r\n\tSpawn:\r\n\t\t{3} A 1\r\n\t\tloop\r\n  }}\r\n}}\r\n";

        private const int BaseActorID = 10000;

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            if (args.Length < 2)
            {
                Console.WriteLine("WDL2WAD - Usage: <Input WDL Path> <Output WAD Path>");
                Console.ReadKey();
                return;
            }

            var wdlPath = args[0];
            if (!Common.IsValidPath(wdlPath) || !File.Exists(wdlPath))
            {
                Console.WriteLine($"\"{wdlPath}\" not found");
                Console.ReadKey();
                return;
            }

            var wadPath = args[1];
            if (!Common.IsValidPath(wadPath))
            {
                Console.WriteLine($"\"{wadPath}\" is not a valid path");
                Console.ReadKey();
                return;
            }

            WDLCompiler compiler = new WDLCompiler()
            {
                ScriptName = "test",
                ShowTokens = false,
                GeneratePropertyList = true
            };

            var wdlDirectory = Path.GetDirectoryName(wdlPath);

            var result = compiler.Parse(wdlPath, out var code);
            if (result == 0)
            {
                var thingsTextures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                var actorID = 0;
                var ackTableContents = new StreamWriter(new MemoryStream(), Encoding.ASCII);
                var decorateContents = new StreamWriter(new MemoryStream(), Encoding.ASCII);

                var uniqueIDs = new Dictionary<string, string>();

                foreach (var thing in compiler.PropertyList["Thing"])
                {
                    AddToDecorate("thing", uniqueIDs, thing, decorateContents, ackTableContents, ref actorID, thingsTextures);
                }

                foreach (var actor in compiler.PropertyList["Actor"])
                {
                    AddToDecorate("actor", uniqueIDs,  actor, decorateContents, ackTableContents, ref actorID, thingsTextures);
                }

                var wad = new WADFileBuilder(true);

                decorateContents.Flush();
                var deciorateData = Common.ReadFully(decorateContents.BaseStream); //why?
                var decorateLump = new Lump(new MemoryStream(deciorateData));
                decorateLump.Name = "DECORATE";
                wad.Add(decorateLump);

                ackTableContents.Flush();
                var ackTableData = Common.ReadFully(ackTableContents.BaseStream); //why?
                var ackTableLump = new Lump(new MemoryStream(ackTableData));
                ackTableLump.Name = "ACKTABLE";
                wad.Add(ackTableLump);

                var mainPalette = new List<byte>();

                foreach (var palette in compiler.PropertyList["Palette"])
                {
                    if (palette.Value.TryGetValue("Palfile", out var palFile) && compiler.PropertyList.TryGetValue("Bmap", out var bmaps))
                    {
                        var bitmapFilename = palFile.First().First().Trim('"');
                        var bitmapPath = $"{wdlDirectory}\\{bitmapFilename}";
                        var loader = LoadBitmap(bitmapPath);
                        if (loader != null)
                        {
                            foreach (var item in loader.Palette)
                            {
                                foreach (var b in item)
                                {
                                    mainPalette.Add(b);
                                }
                            }
                        }
                    }
                }

                if (mainPalette.Count % 256 != 0)
                {
                    Console.WriteLine("Invalid palettes");
                    return;
                }

                var playPalStream = new MemoryStream(mainPalette.ToArray());
                var playPalLump = new Lump(playPalStream);
                playPalLump.Name = "PLAYPAL";
                wad.Add(playPalLump);

                var txStartStream = new MemoryStream(0);
                var txStartLump = new Lump(txStartStream);
                txStartLump.Name = "TX_START";
                wad.Add(txStartLump);

                foreach (var texture in compiler.PropertyList["Texture"])
                {
                    if (texture.Value.TryGetValue("Bmaps", out var textureBmaps) && compiler.PropertyList.TryGetValue("Bmap", out var bmaps))
                    {
                        var scaleXValue = 16f;
                        var scaleYValue = 16f;
                        if (texture.Value.TryGetValue("Scale_xy", out var scaleXY))
                        {
                            scaleXValue = float.Parse(scaleXY.First()[0]);
                            scaleYValue = float.Parse(scaleXY.First()[1]);
                        }
                        if (texture.Value.TryGetValue("Scale_x", out var scaleX))
                        {
                            scaleXValue = float.Parse(scaleX.First()[0]);
                        }
                        if (texture.Value.TryGetValue("Scale_y", out var scaleY))
                        {
                            scaleYValue = float.Parse(scaleY.First()[0]);
                        }
                        if (scaleXValue == 1f)
                        {
                            scaleXValue = 16f;
                        }
                        if (scaleYValue == 1f)
                        {
                            scaleYValue = 16f;
                        }
                        scaleXValue = 16f / scaleXValue;
                        scaleYValue = 16f / scaleYValue;
                        if (bmaps.TryGetValue(textureBmaps.First().First(), out var bmap))
                        {
                            var x = 0;
                            var y = 0;
                            var width = 0;
                            var height = 0;
                            if (bmap.TryGetValue("Options", out var bmapOptions))
                            {
                                var value = bmapOptions.FirstOrDefault();
                                if (value != null)
                                {
                                    x = int.Parse(value[0]);
                                    y = int.Parse(value[1]);
                                    width = int.Parse(value[2]);
                                    height = int.Parse(value[3]);
                                }
                            }
                            if (bmap.TryGetValue("File", out var bmapFile))
                            {
                                var bitmapFilename = bmapFile.First().First().Trim('"');
                                var bitmapPath = $"{wdlDirectory}\\{bitmapFilename}";
                                var loader = LoadBitmap(bitmapPath);
                                if (loader != null)
                                {
                                    Indexmap data;
                                    if (width > 0 && height > 0)
                                    {
                                        var tileIndex = loader.GetTile(x, y, width, height);
                                        data = tileIndex > -1 ? loader.Tiles[tileIndex] : loader.Master;
                                    }
                                    else
                                    {
                                        data = loader.Master;
                                    }
                                    var pixelData = data.Data;
                                    var newWidth = (int)Math.Max(1, data.Width * scaleXValue);
                                    var newHeight = (int)Math.Max(1, data.Height * scaleYValue);
                                    if (newWidth != data.Width || newHeight != data.Height)
                                    {
                                        pixelData = Common.ScaleImage(pixelData, data.Width, data.Height, newWidth, newHeight);
                                    }
                                    var memoryStream = new MemoryStream();
                                    var binaryWriter = new BinaryWriter(memoryStream);
                                    PCXWriter.WritePCX(pixelData, newWidth, newHeight, ExtractPalette(loader), binaryWriter);
                                    binaryWriter.Flush();
                                    var pcxData = Common.ReadFully(memoryStream); //why?
                                    var bitmapLump = new Lump(new MemoryStream(pcxData));
                                    if (thingsTextures.Contains(texture.Key))
                                    {
                                        bitmapLump.Name = $"{GetUniqueID(texture.Key, uniqueIDs)}A1";
                                    }
                                    else
                                    {
                                        bitmapLump.Name = texture.Key.Length > 8 ? texture.Key.Substring(0, 8) : texture.Key;
                                    }
                                    wad.Add(bitmapLump);
                                }
                            }
                        }
                    }
                }

                var txEndStream = new MemoryStream(0);
                var txEndLump = new Lump(txEndStream);
                txEndLump.Name = "TX_END";
                wad.Add(txEndLump);

                using (var wadBinaryWriter = new BinaryWriter(File.Create(wadPath)))
                {
                    var wadFile = wad.Build("DUMMY");
                    var wadData = Common.ReadFully(wadFile.GetStream());
                    wadBinaryWriter.Write(wadData);
                }
            }

            void AddToDecorate(string type, Dictionary<string, string>uniqueIDs, KeyValuePair<string, Dictionary<string, List<List<string>>>> thing, StreamWriter decorateStringBuilder, StreamWriter ackTableStringBuilder, ref int actorID, HashSet<string> thingsTextures)
            {
                if (thing.Value.TryGetValue("Texture", out var thingTexture) && compiler.PropertyList.TryGetValue("Texture", out var textures))
                {
                    var thingTextureName = thingTexture.First().First();
                    if (textures.TryGetValue(thingTextureName, out var texture))
                    {
                        if (texture.TryGetValue("Bmaps", out var textureBmaps) && compiler.PropertyList.TryGetValue("Bmap", out var bmaps))
                        {
                            if (bmaps.TryGetValue(textureBmaps.First().First(), out var bmap))
                            {
                                if (bmap.TryGetValue("File", out var bmapFile))
                                {
                                    thingsTextures.Add(thingTextureName);
                                    var finalID = BaseActorID + actorID++;
                                    ackTableStringBuilder.WriteLine($"{finalID},{type},{thing.Key}");
                                    var textureName = GetUniqueID(thingTextureName, uniqueIDs);
                                    decorateStringBuilder.Write(DecorateActorTemplate, thing.Key, finalID, 16f, textureName);
                                }
                            }
                        }
                    }
                }
            }

            Loader LoadBitmap(string bitmapPath)
            {
                Loader loader;
                try
                {
                    loader = new LbmLoader();
                    if (!loader.Load(bitmapPath))
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    loader = null;
                }
                if (loader == null)
                {
                    try
                    {
                        loader = new PcxLoader();
                        if (!loader.Load(bitmapPath))
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        loader = null;
                    }
                }
                return loader;
            }
        }

        private static string GetUniqueID(string name, Dictionary<string, string> uniqueIDs)
        {
            if (!uniqueIDs.TryGetValue(name, out var id))
            {
                id = SequentialIDGenerator.GenerateNextID();
                uniqueIDs[name] = id;
            }
            return id;
        }

        private static Color[] ExtractPalette(Loader loader)
        {
            var mainPalette = new Color[256];
            for (var i = 0; i < 256; i++)
            {
                mainPalette[i] = new Color(loader.Palette[i][0], loader.Palette[i][1], loader.Palette[i][2]);
            }
            return mainPalette;
        }
    }
}
