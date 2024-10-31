namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个枚举指示单个上传任务的状态
/// </summary>
public enum UploadTaskStatus
{
    /// <summary>
    /// 没有开始上传
    /// </summary>
    None,
    /// <summary>
    /// 正在上传中
    /// </summary>
    Uploading,
    /// <summary>
    /// 上传已成功
    /// </summary>
    Success,
    /// <summary>
    /// 上传发生异常
    /// </summary>
    Exception
}
