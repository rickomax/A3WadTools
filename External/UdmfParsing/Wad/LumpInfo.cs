// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Diagnostics;

namespace UdmfParsing.Wad
{
    [DebuggerDisplay("{ToString()}")]
    public struct LumpInfo : IEquatable<LumpInfo>
    {
        public readonly int Position;
        public readonly int Size;
        public readonly LumpName Name;

        public LumpInfo(int position, int size, LumpName name)
        {
            Position = position;
            Size = size;
            Name = name;
        }

        public override string ToString() => $"{Name} (offset: {Position}, size: {Size})";

        #region Equality
        public bool Equals(LumpInfo other)
        {
            return Position == other.Position && Size == other.Size && Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LumpInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Position;
                hashCode = (hashCode * 397) ^ Size;
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(LumpInfo left, LumpInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LumpInfo left, LumpInfo right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}
