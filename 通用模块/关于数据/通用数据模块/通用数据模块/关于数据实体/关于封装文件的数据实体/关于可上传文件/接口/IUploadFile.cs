namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来封装上传文件
/// </summary>
public interface IUploadFile
{
    #region 用来读取文件的流
    /// <summary>
    /// 打开一个用来读取文件的流
    /// </summary>
    /// <param name="cancellationToken">用来取消异步操作的令牌</param>
    /// <returns></returns>
    Stream OpenFileStream(CancellationToken cancellationToken = default);
    #endregion
    #region 文件的名称
    /// <summary>
    /// 获取文件的名称
    /// </summary>
    string FileName { get; }
    #endregion
    #region 请求内容类型
    /// <summary>
    /// 获取请求的内容类型
    /// </summary>
    string ContentType { get; }
    #endregion
    #region 文件的长度
    /// <summary>
    /// 获取文件的长度，
    /// 以字节为单位
    /// </summary>
    long Length { get; }
    #endregion
}
