namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型封装了剪切板的每一项的数据
/// </summary>
sealed class ClipboardItem
{
    #region 数据的MIME类型
    /// <summary>
    /// 获取数据的MIME类型
    /// </summary>
    public string Type { get; set; }
    #endregion
    #region 数据的大小
    /// <summary>
    /// 获取数据的大小，
    /// 以字节为单位
    /// </summary>
    public long Size { get; set; }
    #endregion
    #region 数据的文本值
    /// <summary>
    /// 获取数据的文本值，
    /// 如果它不是文本，则为<see langword="null"/>
    /// </summary>
    public string? Text { get; set; }
    #endregion
    #region 数据的二进制值
    /// <summary>
    /// 获取数据的二进制值，
    /// 如果它是文本数据，则为<see langword="null"/>
    /// </summary>
    public byte[]? Data { get; set; }
    #endregion
    #region 获取数据的Uri形式
    /// <summary>
    /// 获取数据的Uri形式，
    /// 它可用于图片预览等用途
    /// </summary>
    public string? ObjectURL { get; init; }
    #endregion
}
