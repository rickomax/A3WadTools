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

using MarcelJoachimKloubert.DWAD.WADs.Lumps;
using System;
using System.IO;

namespace MarcelJoachimKloubert.DWAD.WADs
{
    partial class WADFileBase
    {
        internal class UnknownLump : DisposableBase, ILump
        {
            #region Properties (5)

            internal WADFileBase File { get; set; }

            IWADFile ILump.File
            {
                get { return this.File; }
            }

            public string Name
            {
                get;
                internal set;
            }

            public int Position
            {
                get;
                internal set;
            }

            public int Size
            {
                get;
                internal set;
            }

            #endregion Properties (5)

            #region Methods (3)

            public Stream GetStream()
            {
                return this.InvokeForDisposable((obj) =>
                    {
                        var lump = (UnknownLump)obj;

                        return lump.File.InvokeForStream(
                            func: (file, stream, state) =>
                                {
                                    var result = new MemoryStream();
                                    try
                                    {
                                        stream.Position = state.Lump.Position - FILE_ID_SIZE;

                                        var buffer = new byte[state.Lump.Size];
                                        if (buffer.Length > 0)
                                        {
                                            var bytesRead = stream.Read(buffer, 0, buffer.Length);
                                            if (bytesRead > 0)
                                            {
                                                result.Write(buffer, 0, buffer.Length);
                                            }
                                        }

                                        result.Position = 0;
                                        return result;
                                    }
                                    catch (Exception ex)
                                    {
                                        result.Dispose();

                                        throw ex;
                                    }
                                },
                            funcState: new
                            {
                                Lump = lump,
                            });
                    });
            }

            protected override void OnDispose(bool disposing, ref bool isDisposed)
            {
            }

            public override string ToString()
            {
                return (this.Name ?? string.Empty).Trim();
            }

            #endregion Methods (3)
        }
    }
}