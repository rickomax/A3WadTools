// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using UdmfParsing.Common;

namespace UdmfParsing.Udmf
{
    public sealed class UnknownProperty
    {
        public Identifier Name { get; }
        public string Value { get; }

        public UnknownProperty(Identifier name, string value)
        {
            Name = name;
            Value = value;
        }

        public UnknownProperty Clone()
        {
            return new UnknownProperty(Name, Value);
        }
    }
}