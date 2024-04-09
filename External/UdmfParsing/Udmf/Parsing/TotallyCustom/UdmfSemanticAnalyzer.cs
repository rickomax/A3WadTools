// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using UdmfParsing.Common;
using UdmfParsing.Udmf.Parsing.TotallyCustom.AbstractSyntaxTree;

namespace UdmfParsing.Udmf.Parsing.TotallyCustom
{
    public static partial class UdmfSemanticAnalyzer
    {
        public static MapData Process(IEnumerable<IGlobalExpression> result)
        {
            var map = new MapData();

            foreach (var globalExpression in result)
            {
                switch (globalExpression)
                {
                    case Assignment assignment:
                        ProcessGlobalAssignment(map, assignment);
                        break;

                    case Block block:
                        ProcessBlock(map, block);
                        break;
                }
            }

            return map;
        }

        static partial void ProcessGlobalAssignment(MapData map, Assignment assignment);
        static partial void ProcessBlock(MapData map, Block block);


        static UnknownBlock ProcessUnknownBlock(Block block)
        {
            var unknownBlock = new UnknownBlock(block.Name);

            unknownBlock.Properties.AddRange(block.Fields.Select(a => new UnknownProperty(a.Name, a.ValueAsString())));

            return unknownBlock;
        }

        static int ReadIntValue(Assignment assignment, string context) => ReadValue<int, IntegerToken>(assignment, context);
        static double ReadDoubleValue(Assignment assignment, string context) => ReadValue<double, FloatToken>(assignment, context);
        static bool ReadBoolValue(Assignment assignment, string context) => ReadValue<bool, BooleanToken>(assignment, context);
        static string ReadStringValue(Assignment assignment, string context) => ReadValue<string, StringToken>(assignment, context);

        static T ReadValue<T, TToken>(Assignment assignment, string context) where TToken : ValueToken<T>
        {
            if (assignment.Value is TToken t)
            {
                return t.Value;
            }

            throw new ParsingException($"Expected {typeof(T)} for {context} but got {assignment.Value} ({assignment.Value.Location}).");
        }
    }
}