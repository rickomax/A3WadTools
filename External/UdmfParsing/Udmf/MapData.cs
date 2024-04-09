// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.IO;
using System.Text;
namespace UdmfParsing.Udmf
{
    public sealed partial class MapData
    {
        public static MapData LoadFromUsingTotallyCustom(TextReader reader)
        {
            var lexer = new Parsing.TotallyCustom.UdmfLexer(reader);
            var expressions = Parsing.TotallyCustom.UdmfParser.Parse(lexer.Scan());
            return Parsing.TotallyCustom.UdmfSemanticAnalyzer.Process(expressions);
        }

        public static MapData LoadFromUsingTotallyCustom(Stream stream)
        {
            using (var textReader = new StreamReader(stream, Encoding.ASCII))
            {
                return LoadFromUsingTotallyCustom(textReader);
            }
        }
    }
}