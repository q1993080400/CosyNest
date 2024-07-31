namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是选择要上传的文件的时候，
/// 触发的事件的参数
/// </summary>
public sealed record OnSelectFileEventArgs
{
    #region 所有选择的文件
    /// <summary>
    /// 获取所有选择的文件
    /// </summary>
    public required IReadOnlyList<IBrowserFile> SelectFiles { get; init; }
    #endregion
}
