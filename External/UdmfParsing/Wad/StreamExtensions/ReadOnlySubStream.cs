// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.IO;

namespace UdmfParsing.Wad.StreamExtensions
{
    // Based on https://social.msdn.microsoft.com/Forums/vstudio/en-US/c409b63b-37df-40ca-9322-458ffe06ea48/how-to-access-part-of-a-filestream-or-memorystream
    public sealed class ReadOnlySubStream : Stream
    {
        private readonly Stream _baseStream;
        private bool _disposed = false;
        private readonly long _length;
        private long _position;

        public ReadOnlySubStream(Stream baseStream, long position, long length)
        {
            if (baseStream == null) throw new ArgumentNullException(nameof(baseStream));
            if (!baseStream.CanRead) throw new ArgumentException("can't read base stream");
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position));

            _baseStream = baseStream;
            _length = length;

            if (!baseStream.CanSeek)
            {
                throw new NotSupportedException("Cannot sub-stream a non-seekable stream");
            }
            baseStream.Seek(position, SeekOrigin.Begin);
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            CheckDisposed();
            long remaining = _length - _position;
            if (remaining <= 0) return 0;
            if (remaining < count) count = (int)remaining;
            int read = _baseStream.Read(buffer, offset, count);
            _position += read;
            return read;
        }
        private void CheckDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);
        }
        public override long Length => _length;
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override bool CanSeek => false;
        public override long Position
        {
            get => _position;
            set => throw new NotSupportedException();
        }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Flush()
        {
            CheckDisposed(); _baseStream.Flush();
        }

        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();
        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
