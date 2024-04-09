// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace UdmfParsing.Udmf
{
    public sealed class InvalidUdmfException : Exception
    {
        public InvalidUdmfException(string message) : base(message)
        {
        }
    }
}