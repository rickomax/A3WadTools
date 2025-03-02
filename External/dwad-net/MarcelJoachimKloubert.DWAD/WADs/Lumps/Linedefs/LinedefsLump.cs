/**********************************************************************************************************************
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

using MarcelJoachimKloubert.DWAD.WADs.Lumps.Linedefs;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Vertexes;
using System.Collections.Generic;
using System.Linq;
using MarcelJoachimKloubert.DWAD.WADs.Lumps.Sidedefs;
using System.IO;
using System;

namespace MarcelJoachimKloubert.DWAD.WADs
{
    partial class WADFileBase
    {
        internal class LinedefsLump : UnknownLump, ILinedefsLump
        {
            #region Methods (1)

            public IEnumerable<ILinedef> EnumerateLinedefs()
{
                // Get all vertices and sidedefs
                var allVertices = this.File
                                     .EnumerateLumps()
                                     .OfType<IVertexesLump>()
                                     .SelectMany(x => x.EnumerateVertexes())
                                     .ToArray();

                var allSidedefs = this.File
                                      .EnumerateLumps()
                                      .OfType<ISidedefsLump>()
                                      .SelectMany(x => x.EnumerateSidedefs())
                                      .ToArray();

                using (var stream = this.GetStream())
                {
                    byte[] buffer = new byte[14]; // 14 bytes per Linedef

                    while (stream.Read(buffer, 0, buffer.Length) == buffer.Length)
                    {
                        // Parse fields from the buffer
                        var startVertexIndex = BitConverter.ToInt16(buffer, 0);
                        var endVertexIndex = BitConverter.ToInt16(buffer, 2);
                        var flags = BitConverter.ToInt16(buffer, 4);
                        var specialType = BitConverter.ToInt16(buffer, 6);
                        var sectorTag = BitConverter.ToInt16(buffer, 8);
                        var sideDefRight = BitConverter.ToInt16(buffer, 10);
                        var sideDefLeft = BitConverter.ToInt16(buffer, 12);

                        // Validate indices
                        if (startVertexIndex < 0 || startVertexIndex >= allVertices.Length ||
                            endVertexIndex < 0 || endVertexIndex >= allVertices.Length)
                        {
                            throw new InvalidDataException("Invalid vertex index in Linedef.");
                        }

                        // Get vertices
                        var startVertex = allVertices[startVertexIndex];
                        var endVertex = allVertices[endVertexIndex];

                        // Get sidedefs (if valid)
                        var rightSide = sideDefRight >= 0 && sideDefRight < allSidedefs.Length ? allSidedefs[sideDefRight] : null;
                        var leftSide = sideDefLeft >= 0 && sideDefLeft < allSidedefs.Length ? allSidedefs[sideDefLeft] : null;

                        // Yield the Linedef
                        yield return new Linedef()
                        {
                            End = endVertex,
                            Lump = this,
                            Start = startVertex,
                            StartVertexIndex = startVertexIndex,
                            EndVertexIndex = endVertexIndex,
                            RightSide = rightSide,
                            LeftSide = leftSide,
                            RightSideIndex = sideDefRight,
                            LeftSideIndex = sideDefLeft,
                            Flags = flags,
                            SectorTag = sectorTag,
                            SpecialType = specialType
                        };
                    }
                }
            }

            #endregion Methods (1)
        }
    }
}