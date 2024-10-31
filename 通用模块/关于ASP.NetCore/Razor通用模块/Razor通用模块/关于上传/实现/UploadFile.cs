using System.NetFrancis.Http;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IUploadFile"/>的实现，
/// 可以作为一个前端上传文件的参数
/// </summary>
/// <param name="file">要封装的浏览器上传文件</param>
/// <param name="maxAllowedSize">可以读取的最大字节数，
/// 超过这个限制会引发异常</param>
/// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
sealed class UploadFile(IBrowserFile file, long maxAllowedSize, CancellationToken cancellationToken) : IUploadFile
{
    #region 获取文件流
    public Stream FileStream()
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
