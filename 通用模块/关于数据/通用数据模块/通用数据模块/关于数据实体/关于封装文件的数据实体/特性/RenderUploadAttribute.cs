namespace System.DataFrancis;

/// <summary>
/// 这个特性可以用来修饰类型为<see cref="IUploadFile"/>，
/// 或它的集合的属性，它描述对于上传的限制
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class RenderUploadAttribute : Attribute
{
    #region 可接受文件的类型
    /// <summary>
    /// 描述可接受文件的类型，
    /// 它的语法和Web的input标签中的accept属性相同
    /// </summary>
    public required string Accept { get; init; }
    #endregion
    #region 上传按钮文字
    /// <summary>
    /// 获取上传按钮的文字
    /// </summary>
    public string UploadButtonText { get; init; } = "上传文件";
    #endregion
    #region 可上传文件的最大大小
    /// <summary>
    /// 获取可上传文件的最大大小，
    /// 以字节为单位，默认为50M
    /// </summary>
    public long MaxAllowedSize { get; init; } = 1024 * 1024 * 50;
    #endregion
    #region 是否启用从剪切板上传功能
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则启用从剪切板上传的功能
    /// </summary>
    public bool EnableClipboardUpload { get; init; }
    #endregion
}
