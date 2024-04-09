// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;

namespace UdmfParsing.Common
{
    [DebuggerDisplay("{" + nameof(_name) + "}")]
    public sealed class Identifier : IEquatable<Identifier>, IEquatable<string>
    {
        private readonly string _name;

        public Identifier(string name) => _name = name;
        public string ToLower() => _name.ToLowerInvariant();
        public override string ToString() => _name;
        public static explicit operator string(Identifier id) => id._name;

        #region Equality

        public bool Equals(Identifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StringComparer.OrdinalIgnoreCase.Equals(_name, other._name);
        }

        public bool Equals(string other)
        {
            if (ReferenceEquals(null, other)) return false;
            return StringComparer.OrdinalIgnoreCase.Equals(_name, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return 
                (obj is Identifier other && Equals(other)) ||
                (obj is string s && Equals(s));
        }

        public override int GetHashCode() => ToLower().GetHashCode();
        public static bool operator ==(Identifier left, Identifier right) => Equals(left, right);
        public static bool operator !=(Identifier left, Identifier right) => !Equals(left, right);

        #endregion 
    }
}