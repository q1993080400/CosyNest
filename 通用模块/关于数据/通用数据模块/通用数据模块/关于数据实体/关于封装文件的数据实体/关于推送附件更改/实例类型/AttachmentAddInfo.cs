namespace System.DataFrancis;

/// <summary>
/// 这个记录是一个添加附件的参数
/// </summary>
/// <inheritdoc cref="IPushAttachmentChange{Obj}"/>
public sealed record AttachmentAddInfo<Obj>
{
    #region 要添加的附件
    /// <summary>
    /// 获取要添加的附件
    /// </summary>
    public required IHasUploadFileServer UploadFile { get; init; }
    #endregion
    #region 附件所依附的对象
    /// <summary>
    /// 获取附件所依附的对象
    /// </summary>
    public required Obj Depend { get; init; }
    #endregion
}
