using System.Diagnostics.CodeAnalysis;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是专门为<see cref="UploadFileServer"/>设计的<see cref="Stream"/>，
/// 它能够正确地在服务端渲染中进行文件上传工作
/// </summary>
/// <param name="file">要封装的浏览器文件</param>
/// <param name="maxAllowedSize">可上传文件的最大大小，以字节为单位</param>
sealed class LazyBrowserFileStream(IBrowserFile file, long maxAllowedSize) : Stream
{
    private Stream? UnderlyingStream { get; set; }
    private bool IsDisposed { get; set; }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => file.Size;

    public override long Position
    {
        get => UnderlyingStream?.Position ?? 0;
        set => throw new NotSupportedException();
    }

    public override void Flush() => UnderlyingStream?.Flush();

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count,
        CancellationToken cancellationToken)
    {
        EnsureStreamIsOpen();
        return UnderlyingStream.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        EnsureStreamIsOpen();
        return UnderlyingStream.ReadAsync(buffer, cancellationToken);
    }

    [MemberNotNull(nameof(UnderlyingStream))]
    private void EnsureStreamIsOpen() =>
        UnderlyingStream ??= file.OpenReadStream(maxAllowedSize);

    protected override void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;
        UnderlyingStream?.Dispose();
        IsDisposed = true;
        base.Dispose(disposing);
    }

    public override int Read(byte[] buffer, int offset, int count)
        => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin)
        => throw new NotSupportedException();

    public override void SetLength(long value)
        => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
        => throw new NotSupportedException();
}