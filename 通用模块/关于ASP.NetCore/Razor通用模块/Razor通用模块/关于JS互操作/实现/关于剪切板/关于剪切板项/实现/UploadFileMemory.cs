namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IUploadFile"/>的实现，
/// 它通过内存流来读取数据
/// </summary>
/// <param name="FileName">文件的名称</param>
/// <param name="ContentType">文件的内容类型</param>
/// <param name="Length">文件的长度，以字节为单位</param>
/// <param name="Stream">用来读取文件的流</param>
sealed record UploadFileMemory(string FileName, string ContentType, long Length, Stream Stream) : IUploadFile
{
    #region 获取文件流
    public Stream OpenFileStream(CancellationToken cancellationToken = default)
        => Stream;
    #endregion
}
