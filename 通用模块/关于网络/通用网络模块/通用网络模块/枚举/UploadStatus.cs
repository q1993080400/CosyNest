namespace System.NetFrancis;

/// <summary>
/// 这个枚举指示文件的上传状态
/// </summary>
public enum UploadStatus
{
    /// <summary>
    /// 表示没有需要上传的文件
    /// </summary>
    None,
    /// <summary>
    /// 表示文件正在上传
    /// </summary>
    Uploading,
    /// <summary>
    /// 表示文件已经上传完毕
    /// </summary>
    UploadCompleted,
    /// <summary>
    /// 表示文件上传失败
    /// </summary>
    UploadFailed
}
