namespace System.DataFrancis;

/// <summary>
/// 这个抽象记录为所有实现<see cref="IHasReadOnlyPreviewFile"/>接口的类型提供基类
/// </summary>
abstract record HasPreviewFileBase : IHasReadOnlyPreviewFile
{
    #region 文件的ID
    public required Guid ID { get; init; }
    #endregion
    #region 封面Uri
    public required string CoverUri { get; set; }
    #endregion
    #region 本体Uri
    public required string Uri { get; set; }
    #endregion
    #region 文件的名称
    public required string FileName { get; init; }
    #endregion
    #region 存储层文件名称
    public virtual string FileNameStorage
        => Uri.Op().GetEndPoint();
    #endregion
    #region 是否启用该文件
    public bool IsEnable { get; set; } = true;
    #endregion
    #region 禁用文件
    public void Disable()
        => IsEnable = false;
    #endregion
}
