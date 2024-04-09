// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace UdmfParsing.Udmf
{
    public sealed partial class LineDef : IEquatable<LineDef>
    {
        #region Equality members (based on V1 and V2)

        public bool Equals(LineDef other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _v1 == other._v1 && _v2 == other._v2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LineDef && Equals((LineDef) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_v1 * 397) ^ _v2;
            }
        }

        #endregion
    }
}