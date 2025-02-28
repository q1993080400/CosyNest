namespace System.DataFrancis;

/// <summary>
/// 这个记录是一个删除附件的参数
/// </summary>
/// <inheritdoc cref="IPushAttachmentChange{Obj}"/>
public sealed record AttachmentDeleteInfo<Obj>
{
    #region 文件的物理位置
    /// <summary>
    /// 获取要删除的文件所在的物理位置
    /// </summary>
    public required IFilePosition FilePosition { get; init; }
    #endregion
    #region 文件所依附的对象
    /// <summary>
    /// 获取文件所依附的对象
    /// </summary>
    public required Obj Depend { get; init; }
    #endregion
}
