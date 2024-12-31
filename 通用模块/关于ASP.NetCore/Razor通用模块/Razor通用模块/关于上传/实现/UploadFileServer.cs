namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IUploadFile"/>专门用于服务端渲染的实现，
/// 它可以在Server模式下正确地上传文件
/// </summary>
/// <param name="file">要封装的浏览器文件</param>
/// <param name="maxAllowedSize">可上传文件的最大大小，以字节为单位</param>
sealed class UploadFileServer(IBrowserFile file, long maxAllowedSize) : IUploadFile
{
    #region 公开成员
    #region 获取文件流
    public Stream OpenFileStream(CancellationToken cancellationToken = default)
        => Setem;
    #endregion
    #region 文件的名称
    public string FileName
        => file.Name;
    #endregion
    #region 文件的内容类型
    public string ContentType
        => file.ContentType;
    #endregion
    #region 文件的长度
    public long Length
        => file.Size;
    #endregion
    #endregion
    #region 内部成员
    #region 上传文件流
    /// <summary>
    /// 获取用来上传的文件流
    /// </summary>
    private LazyBrowserFileStream Setem { get; } = new(file, maxAllowedSize);
    #endregion
    #endregion
}
