namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IUploadFile"/>的实现，
/// 可以作为一个前端上传文件的参数
/// </summary>
/// <param name="file">要封装的浏览器上传文件</param>
sealed class UploadFile(IBrowserFile file) : IUploadFile
{
    #region 获取文件流
    public Stream OpenFileStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
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
