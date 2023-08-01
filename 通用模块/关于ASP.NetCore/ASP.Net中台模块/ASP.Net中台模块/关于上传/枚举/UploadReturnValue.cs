namespace Microsoft.AspNetCore;

/// <summary>
/// 这个枚举是上传中间件的返回值，
/// 它指示是否可以处理上传，以及上传的结果
/// </summary>
public enum UploadReturnValue
{
    /// <summary>
    /// 表示该中间件不支持处理这个请求
    /// </summary>
    NotSupported,
    /// <summary>
    /// 表示上传成功
    /// </summary>
    Success,
    /// <summary>
    /// 表示上传失败
    /// </summary>
    Fail
}
