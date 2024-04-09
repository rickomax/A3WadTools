// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UdmfParsing.Wad
{
    [DebuggerDisplay("{ToString()}")]
    public sealed class LumpName : IEquatable<LumpName>
    {
        public const int MaxLength = 8;
        private readonly string _name;

        public LumpName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

            if (Regex.IsMatch(name, @"[^A-Z0-9\[\]\-_]", RegexOptions.Compiled))
            {
                throw new ArgumentException($"'{name}' has invalid characters.",nameof(name));
            }

            if (name.Length > MaxLength)
            {
                throw new ArgumentException($"'{name}' is too long.",nameof(name));
            }
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        public static implicit operator LumpName(string name)
        {
            return new LumpName(name);
        }

        #region Equality stuff
        public bool Equals(LumpName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LumpName && Equals((LumpName)obj);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public static bool operator ==(LumpName left, LumpName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LumpName left, LumpName right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
