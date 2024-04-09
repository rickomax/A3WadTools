﻿/**********************************************************************************************************************
 * dwad-net (https://github.com/mkloubert/dwad-net)                                                                   *
 *                                                                                                                    *
 * Copyright (c) 2015, Marcel Joachim Kloubert <marcel.kloubert@gmx.net>                                              *
 * All rights reserved.                                                                                               *
 *                                                                                                                    *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the   *
 * following conditions are met:                                                                                      *
 *                                                                                                                    *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the          *
 *    following disclaimer.                                                                                           *
 *                                                                                                                    *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the       *
 *    following disclaimer in the documentation and/or other materials provided with the distribution.                *
 *                                                                                                                    *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote    *
 *    products derived from this software without specific prior written permission.                                  *
 *                                                                                                                    *
 *                                                                                                                    *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, *
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  *
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, *
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR    *
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,  *
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE   *
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                           *
 *                                                                                                                    *
 **********************************************************************************************************************/

using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sidedefs;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Vertexes;

namespace MarcelJoachimKloubert.DWAD.WADs.Lumps.Linedefs
{
    /// <summary>
    /// Describes a line definition.
    /// </summary>
    public interface ILinedef
    {
        #region Properties (4)

        /// <summary>
        /// Gets the end vertex.
        /// </summary>
        IVertex End { get; }

        /// <summary>
        /// Gets the length between <see cref="ILinedef.Start" /> and <see cref="ILinedef.End" />.
        /// </summary>
        double Length { get; }

        /// <summary>
        /// Gets the underlying lump.
        /// </summary>
        ILinedefsLump Lump { get; }

        /// <summary>
        /// Gets the start vertex.
        /// </summary>
        IVertex Start { get; }

        /// <summary>
        /// 
        /// </summary>
        ISidedef RightSide { get; }

        /// <summary>
        /// 
        /// </summary>
        ISidedef LeftSide { get; }
        /// <summary>
        /// 
        /// </summary>
        short Flags { get; }

        /// <summary>
        /// 
        /// </summary>
        short SectorTag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        short SpecialType { get; }

        /// <summary>
        /// 
        /// </summary>
        short RightSideIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        short LeftSideIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        short StartVertexIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        short EndVertexIndex { get; }

        /// <summary>
        /// 
        /// </summary>
        int Arg0 { get; }

        /// <summary>
        /// 
        /// </summary>
        int Arg1 { get; }

        /// <summary>
        /// 
        /// </summary>
        int Arg2 { get; }

        /// <summary>
        /// 
        /// </summary>
        int Arg3 { get; }


        /// <summary>
        /// 
        /// </summary>
        int Arg4 { get; }

        #endregion Properties (4)
    }
}