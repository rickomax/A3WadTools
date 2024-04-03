using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using VCCCompiler;

namespace WDL2WAD
{
    internal class Program
    {
        private const string DecorateActorTemplate = "Actor {0} : DoomImp {1}\r\n{{\r\n  Radius {2}\r\n  States\r\n  {{\r\n\tSpawn:\r\n\t\t{3} A 1\r\n\t\tloop\r\n  }}\r\n}}";

        private const int BaseActorID = 10000;

        private static bool IsValidPath(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var filename = Path.GetFileName(path);
            var invalidChars = Path.GetInvalidFileNameChars();
            return directory != null && filename.IndexOfAny(invalidChars) < 0;
        }

        private static ushort CalculateHash(string input)
        {
            var hash = 0;
            foreach (var c in input)
            {
                hash = c + (hash << 6) + (hash << 16) - hash;
            }
            return (ushort)(hash & 0xFFFF);
        }

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
            if (!IsValidPath(wdlPath) || !File.Exists(wdlPath))
            {
                Console.WriteLine($"\"{wdlPath}\" not found");
                Console.ReadKey();
                return;
            }

            var wadPath = args[1];
            if (!IsValidPath(wadPath))
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

            var result = compiler.Parse(wdlPath, out var code);
            if (result == 0)
            {
                var decorateStringBuilder = new StringBuilder();
                foreach (var thing in compiler.PropertyList["Thing"])
                {
                    AddToDecorate(thing, decorateStringBuilder);
                }
                foreach (var actor in compiler.PropertyList["Actor"])
                {
                    AddToDecorate(actor, decorateStringBuilder);
                }
                var y = decorateStringBuilder.ToString();
                var x = 1;
            }

            void AddToDecorate(KeyValuePair<string, Dictionary<string, List<List<string>>>> thing, StringBuilder decorateStringBuilder)
            {
                if (thing.Value.TryGetValue("Texture", out var thingTexture) && compiler.PropertyList.TryGetValue("Texture", out var textures))
                {
                    if (textures.TryGetValue(thingTexture.First().First(), out var texture))
                    {
                        if (texture.TryGetValue("Bmaps", out var textureBmaps) && compiler.PropertyList.TryGetValue("Bmap", out var bmaps))
                        {
                            if (bmaps.TryGetValue(textureBmaps.First().First(), out var bmap))
                            {
                                if (bmap.TryGetValue("File", out var bmapFile))
                                {
                                    var hash = BaseActorID+ CalculateHash(thing.Key);
                                    var graphicName = Path.GetFileNameWithoutExtension(bmapFile.First().First().Trim('"'));
                                    decorateStringBuilder.AppendFormat(DecorateActorTemplate, thing.Key, hash, 16f, graphicName);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
