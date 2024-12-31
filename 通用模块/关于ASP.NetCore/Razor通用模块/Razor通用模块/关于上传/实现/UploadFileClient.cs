namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IUploadFile"/>专门用于客户端渲染的实现，
/// 它可以在WebAssembly模式下正确地上传文件
/// </summary>
/// <param name="file">要封装的浏览器上传文件</param>
/// <param name="maxAllowedSize">指示上传文件的最大大小，以字节为单位</param>
sealed class UploadFileClient(IBrowserFile file, long maxAllowedSize) : IUploadFile
{
    #region 获取文件流
    public Stream OpenFileStream(CancellationToken cancellationToken = default)
        => file.OpenReadStream(maxAllowedSize, cancellationToken);
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
}
