namespace System.DataFrancis;

/// <summary>
/// 这个记录描述了一条可预览文件依附于哪个对象，
/// 以及它的其他信息
/// </summary>
public sealed record PreviewFileInfo
{
    #region 属性所依附的对象
    /// <summary>
    /// 获取这个属性所依附的对象
    /// </summary>
    public required object Target { get; init; }
    #endregion
    #region 可预览文件属性的描述
    /// <summary>
    /// 获取可预览文件的属性的描述
    /// </summary>
    public required IHasPreviewFilePropertyDirectInfo PreviewFilePropertyInfo { get; init; }
    #endregion
}
