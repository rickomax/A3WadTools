// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Immutable;
using UdmfParsing.Common;

namespace UdmfParsing.Udmf.Parsing.TotallyCustom.AbstractSyntaxTree
{
    public sealed class Block : IGlobalExpression, IEquatable<Block>
    {
        public readonly Identifier Name;
        public readonly ImmutableArray<Assignment> Fields;

        public Block(Identifier name, ImmutableArray<Assignment> fields)
        {
            Name = name;
            Fields = fields;
        }

        #region Equality
        public bool Equals(Block other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name.Equals(other.Name) && Fields.Equals(other.Fields);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Block other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode() * 397) ^ Fields.GetHashCode();
            }
        }

        public static bool operator ==(Block left, Block right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Block left, Block right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}