// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.IO;
using System.Text;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.TotallyCustom
{
    public sealed class UdmfLexer
    {
        private readonly TextReader _reader;
        private FilePosition _currentPosition = FilePosition.StartOfFile();
        private const char Null = '\0';
        private StringBuilder _tokenBuffer = new StringBuilder();

        public UdmfLexer(TextReader reader) => _reader = reader;

        public IEnumerable<Token> Scan()
        {
            char next = PeekChar();
            while (next != Null)
            {
                switch (next)
                {
                    case '=':
                        yield return new EqualsToken(_currentPosition);
                        SkipChar();
                        break;
                    case ';':
                        yield return new SemicolonToken(_currentPosition);
                        SkipChar();
                        break;
                    case '{':
                        yield return new OpenBraceToken(_currentPosition);
                        SkipChar();
                        break;
                    case '}':
                        yield return new CloseBraceToken(_currentPosition);
                        SkipChar();
                        break;

                    case char digit when char.IsDigit(next):
                        yield return LexNumber(digit);
                        break;
                    case '-':
                    case '+':
                        yield return LexNumber(next);
                        break;

                    case '"':
                        yield return LexString();
                        break;

                    case char idStart when char.IsLetter(next):
                    case '_':
                        yield return LexIdentifier();
                        break;

                    case '/':
                        SkipComment();
                        break;

                    case '\n':
                        SkipChar();
                        _currentPosition = _currentPosition.NextLine();
                        break;

                    case char ws when char.IsWhiteSpace(next):
                        SkipChar();
                        break;

                    case Null:
                    default:
                        yield break;
                }

                next = PeekChar();
            }
        }

        private Token LexNumber(char first)
        {
            var start = _currentPosition;
            ConsumeChar();

            if (first == '0' && PeekChar() == 'x')
            {
                _tokenBuffer.Clear();
                SkipChar();

                bool IsHexChar(char c) =>
                    char.IsDigit(c) ||
                    (c >= 'a' && c <= 'f') ||
                    (c >= 'A' && c <= 'F');

                if (!IsHexChar(PeekChar()))
                {
                    throw new ParsingException("Malformed hex number: " + _currentPosition);
                }

                while (IsHexChar(PeekChar()))
                {
                    ConsumeChar();
                }
                return new IntegerToken(start, BufferAsHexInteger());
            }

            while (char.IsDigit(PeekChar()))
            {
                ConsumeChar();
            }
            if (PeekChar() != '.')
            {
                return new IntegerToken(start, BufferAsInteger());
            }

            ConsumeChar();

            while (char.IsDigit(PeekChar()))
            {
                ConsumeChar();
            }

            return new FloatToken(start, BufferAsFloat());
        }

        private StringToken LexString()
        {
            var start = _currentPosition;
            SkipChar();

            while (PeekChar() != '"')
            {
                ConsumeChar();
            }
            SkipChar();

            return new StringToken(start, BufferAsString());
        }

        private Token LexIdentifier()
        {
            var start = _currentPosition;
            ConsumeChar();

            while (char.IsLetterOrDigit(PeekChar()) || PeekChar() == '_')
            {
                ConsumeChar();
            }

            var name = BufferAsString();
            switch (name)
            {
                case "true": return new BooleanToken(start, true);
                case "false": return new BooleanToken(start, false);
                default: return new IdentifierToken(start, new Identifier(name));
            }
        }

        private void SkipComment()
        {
            var start = _currentPosition;
            SkipChar();
            switch (PeekChar())
            {
                case '/':
                    SkipChar();
                    while (PeekChar() != '\n' && PeekChar() != Null)
                    {
                        SkipChar();
                    }
                    break;
                case '*':
                    SkipChar();
                    bool inside = true;
                    while (inside)
                    {
                        while (PeekChar() != '*')
                        {
                            SkipChar();
                        }
                        SkipChar();
                        if (PeekChar() == '/')
                        {
                            SkipChar();
                            inside = false;
                        }
                    }
                    break;
                default:
                    throw new ParsingException("Malformed comment on " + start);
            }
        }

        private char PeekChar()
        {
            var next = _reader.Peek();
            return next > -1 ? (char)next : Null;
        }

        private void SkipChar()
        {
            _currentPosition = _currentPosition.NextChar();
            if (_reader.Read() == Null)
            {
                throw new ParsingException("Unexpected end of file at " + _currentPosition);
            }
        }

        private void ConsumeChar()
        {
            _currentPosition = _currentPosition.NextChar();
            var next = _reader.Read();
            if (next == -1)
            {
                throw new ParsingException("Unexpected end of file at " + _currentPosition);
            }
            _tokenBuffer.Append((char)next);
        }

        private int BufferAsHexInteger()
        {
            var value = int.Parse(_tokenBuffer.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
            _tokenBuffer.Clear();
            return value;
        }

        private int BufferAsInteger()
        {
            var value = int.Parse(_tokenBuffer.ToString());
            _tokenBuffer.Clear();
            return value;
        }

        private double BufferAsFloat()
        {
            var value = double.Parse(_tokenBuffer.ToString());
            _tokenBuffer.Clear();
            return value;
        }

        private string BufferAsString()
        {
            var value = _tokenBuffer.ToString();
            _tokenBuffer.Clear();
            return value;
        }
    }
}