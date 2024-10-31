namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录可以用来作为预览上传文件的参数，
/// 它只对图片或视频生效
/// </summary>
public sealed record PreviewUploadMediumInfo
{
    #region 预览图的Uri
    /// <summary>
    /// 获取预览图的Uri
    /// </summary>
    public required string PreviewImageUri { get; init; }
    #endregion
    #region 是否被取消
    private bool IsCancelField;

    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个文件已经被取消，
    /// 它不会再被上传
    /// </summary>
    public bool IsCancel => IsCancelField;
    #endregion
    #region 不再上传这个文件
    /// <summary>
    /// 指示不要再上传这个文件
    /// </summary>
    public void Cancel()
        => IsCancelField = true;
    #endregion
}
