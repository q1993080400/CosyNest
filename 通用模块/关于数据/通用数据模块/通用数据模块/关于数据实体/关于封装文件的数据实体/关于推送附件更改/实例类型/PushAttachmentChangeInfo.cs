namespace System.DataFrancis;

/// <summary>
/// 这个记录可以为<see cref="IPushAttachmentChange"/>提供附件被更改的信息
/// </summary>
public sealed record PushAttachmentChangeInfo
{
    #region 应当删除的附件
    /// <summary>
    /// 获取应该删除的附件
    /// </summary>
    public required IReadOnlyCollection<IHasPreviewFile> Delete { get; init; }
    #endregion
    #region 应当添加的附件
    /// <summary>
    /// 获取应该添加的附件
    /// </summary>
    public required IReadOnlyCollection<IHasUploadFileServer> Add { get; init; }
    #endregion
}
