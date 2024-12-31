namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是在在客户端上传文件时的选项
/// </summary>
public sealed record UploadFileOptions
{
    #region 可上传文件的最大大小
    /// <summary>
    /// 获取可上传文件的最大大小，
    /// 以字节为单位，默认为500KB
    /// </summary>
    public long MaxAllowedSize { get; init; } = 512000;
    #endregion
    #region 格式化最大文件大小
    /// <summary>
    /// 获取最大文件大小的格式化文本形式
    /// </summary>
    public string MaxAllowedSizeFormat
    {
        get
        {
            var maxAllowedSize = (decimal)MaxAllowedSize;
            var (size, unit) = maxAllowedSize switch
            {
                >= 1024 * 1024 * 1024 => (maxAllowedSize / (1024 * 1024 * 1024), "GB"),
                >= 1024 * 1024 => (maxAllowedSize / (1024 * 1024), "MB"),
                >= 1024 => (maxAllowedSize / 1024, "KB"),
                _ => (maxAllowedSize, "字节")
            };
            return size.FormatCommon() + unit;
        }
    }
    #endregion
}
