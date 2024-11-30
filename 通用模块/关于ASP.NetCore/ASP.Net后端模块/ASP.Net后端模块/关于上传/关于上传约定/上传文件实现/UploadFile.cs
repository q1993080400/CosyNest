namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是<see cref="IUploadFile"/>的实现，
/// 可以视为一个后端上传文件的参数
/// </summary>
/// <param name="formFile">要封装的浏览器上传文件</param>
sealed class UploadFile(IFormFile formFile) : IUploadFile
{
    #region 获取文件流
    public Stream OpenFileStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        => formFile.OpenReadStream();
    #endregion
    #region 文件的名称
    public string FileName
        => formFile.FileName;
    #endregion
    #region 文件的内容类型
    public string ContentType
        => formFile.ContentType;
    #endregion
    #region 文件的长度
    public long Length
        => formFile.Length;
    #endregion 
}
