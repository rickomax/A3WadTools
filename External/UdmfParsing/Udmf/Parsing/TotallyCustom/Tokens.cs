// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.TotallyCustom
{
    public abstract class Token
    {
        public FilePosition Location { get; }
        protected Token(FilePosition location) => Location = location;
    }

    public abstract class ValueToken<T> : Token
    {
        public T Value { get; }
        protected ValueToken(FilePosition location, T value)
            : base(location)
        {
            Value = value;
        }

        public override string ToString() => $"{GetType().Name}: {Value}";
    }

    public sealed class IntegerToken : ValueToken<int>
    {
        public IntegerToken(FilePosition location, int value) : base(location, value) { }
    }

    public sealed class FloatToken : ValueToken<double>
    {
        public FloatToken(FilePosition location, double value) : base(location, value) { }
    }

    public sealed class BooleanToken : ValueToken<bool>
    {
        public BooleanToken(FilePosition location, bool value) : base(location, value) { }
    }

    public sealed class StringToken : ValueToken<string>
    {
        public StringToken(FilePosition location, string value) : base(location, value) { }
    }

    public sealed class IdentifierToken : Token
    {
        public Identifier Id { get; }
        public IdentifierToken(FilePosition location, Identifier id) : base(location) => Id = id;
    }

    public sealed class EqualsToken : Token
    {
        public EqualsToken(FilePosition location) : base(location) { }
    }

    public sealed class SemicolonToken : Token
    {
        public SemicolonToken(FilePosition location) : base(location) { }
    }
    
    public sealed class OpenBraceToken : Token
    {
        public OpenBraceToken(FilePosition location) : base(location) { }
    }    

    public sealed class CloseBraceToken : Token
    {
        public CloseBraceToken(FilePosition location) : base(location) { }
    }
}