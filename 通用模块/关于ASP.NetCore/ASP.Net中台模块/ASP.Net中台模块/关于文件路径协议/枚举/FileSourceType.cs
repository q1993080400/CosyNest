namespace Microsoft.AspNetCore;

/// <summary>
/// 这个枚举表示文件源的类型
/// </summary>
public enum FileSourceType
{
    /// <summary>
    /// 表示一个Web图片
    /// </summary>
    WebImage,
    /// <summary>
    /// 表示一个Web视频或音频
    /// </summary>
    WebVideo,
    /// <summary>
    /// 表示它是其他类型的所有文件
    /// </summary>
    File
}
