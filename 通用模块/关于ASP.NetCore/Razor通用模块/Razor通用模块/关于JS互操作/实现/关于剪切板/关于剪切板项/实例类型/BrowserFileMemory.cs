namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是<see cref="IBrowserFile"/>的实现，
/// 它专门用于在内存中读取浏览器文件
/// </summary>
/// <param name="Name">文件名</param>
/// <param name="LastModified">上一次修改的时间</param>
/// <param name="Size">文件大小</param>
/// <param name="ContentType">文件标头</param>
/// <param name="Stream">要读取的文件流</param>
sealed record BrowserFileMemory(string Name, DateTimeOffset LastModified, long Size, string ContentType, Stream Stream) : IBrowserFile
{
    #region 打开文件流
    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        => Stream;
    #endregion
}
