// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using UdmfParsing.Common;
using UdmfParsing.Udmf.Parsing.TotallyCustom.AbstractSyntaxTree;

namespace UdmfParsing.Udmf.Parsing.TotallyCustom
{
    public static class UdmfParser
    {
        private enum ExpectedToken
        {
            Identifier,
            Equals,
            OpenBraceOrEquals,
            Value,
            Semicolon,
            CloseBraceOrIdentifier,
        }

        public static IEnumerable<IGlobalExpression> Parse(IEnumerable<Token> tokens)
        {
            var expected = ExpectedToken.Identifier;
            bool insideBlock = false;

            IdentifierToken assignmentName = null;
            IdentifierToken blockName = null;
            Token valueToken = null;
            List<Assignment> assignments = new List<Assignment>();

            foreach (var token in tokens)
            {
                switch (expected)
                {
                    case ExpectedToken.Identifier:
                        if (token is IdentifierToken id)
                        {
                            blockName = id;
                            expected = ExpectedToken.OpenBraceOrEquals;
                        }
                        else
                        {
                            throw new ParsingException($"Unexpected token {token} on {token.Location}");
                        }
                        break;
                    case ExpectedToken.Equals:
                        if (token is EqualsToken eq)
                        {
                            expected = ExpectedToken.Value;
                        }
                        else
                        {
                            throw new ParsingException($"Unexpected token {token} on {token.Location}");
                        }
                        break;
                    case ExpectedToken.OpenBraceOrEquals:
                        switch (token)
                        {
                            case OpenBraceToken o:
                                insideBlock = true;
                                expected = ExpectedToken.CloseBraceOrIdentifier;
                                break;
                            case EqualsToken e:
                                assignmentName = blockName;
                                blockName = null;
                                expected = ExpectedToken.Value;
                                break;
                            default:
                                throw new ParsingException($"Unexpected token {token} on {token.Location}");
                        }
                        break;
                    case ExpectedToken.Value:
                        switch (token)
                        {
                            case IntegerToken i:
                                valueToken = i;
                                expected = ExpectedToken.Semicolon;
                                break;
                            case FloatToken f:
                                valueToken = f;
                                expected = ExpectedToken.Semicolon;
                                break;
                            case BooleanToken b:
                                valueToken = b;
                                expected = ExpectedToken.Semicolon;
                                break;
                            case StringToken s:
                                valueToken = s;
                                expected = ExpectedToken.Semicolon;
                                break;
                            default:
                                throw new ParsingException($"Unexpected token {token} on {token.Location}");
                        }
                        break;
                    case ExpectedToken.Semicolon:
                        if (token is SemicolonToken)
                        {
                            if (insideBlock)
                            {
                                assignments.Add(new Assignment(assignmentName.Id, valueToken));
                                expected = ExpectedToken.CloseBraceOrIdentifier;
                            }
                            else
                            {
                                yield return new Assignment(assignmentName.Id, valueToken);
                                expected = ExpectedToken.Identifier;
                            }
                        }
                        else
                        {
                            throw new ParsingException($"Unexpected token {token} on {token.Location}");
                        }
                        break;
                    case ExpectedToken.CloseBraceOrIdentifier:
                        switch (token)
                        {
                            case IdentifierToken i:
                                assignmentName = i;
                                expected = ExpectedToken.Equals;
                                break;
                            case CloseBraceToken cb:
                                insideBlock = false;
                                expected = ExpectedToken.Identifier;
                                yield return new Block(blockName.Id, assignments.ToImmutableArray());
                                assignments.Clear();
                                break;
                            default:
                                throw new ParsingException($"Unexpected token {token} on {token.Location}");
                        }
                        break;
                    default:
                        throw new ParsingException($"Unexpected token {token} on {token.Location}");
                }
            }

            if(expected != ExpectedToken.Identifier)
            {
                throw new ParsingException("Unexpected end of file");
            }
        }
    }
}