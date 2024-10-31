namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个强类型Http请求中，
/// 表示上传文件的参数，它会被进行特殊的处理
/// </summary>
public interface IUploadFile
{
    #region 用来读取文件的流
    /// <summary>
    /// 获取用来读取文件的流
    /// </summary>
    Stream FileStream();
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
