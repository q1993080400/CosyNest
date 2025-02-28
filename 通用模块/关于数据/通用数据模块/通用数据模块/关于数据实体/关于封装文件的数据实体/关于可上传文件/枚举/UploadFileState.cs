namespace System.DataFrancis;

/// <summary>
/// 这个枚举表示文件上传的状态
/// </summary>
public enum UploadFileState
{
    /// <summary>
    /// 表示上传未开始
    /// </summary>
    NotStarted,
    /// <summary>
    /// 表示正在进行上传
    /// </summary>
    Uploading,
    /// <summary>
    /// 表示上传已经成功
    /// </summary>
    Success,
    /// <summary>
    /// 表示上传发生错误
    /// </summary>
    Error,
    /// <summary>
    /// 表示上传已成功，
    /// 但是这个文件后来被删除了，
    /// 无法再次找回
    /// </summary>
    FileNotExist
}
