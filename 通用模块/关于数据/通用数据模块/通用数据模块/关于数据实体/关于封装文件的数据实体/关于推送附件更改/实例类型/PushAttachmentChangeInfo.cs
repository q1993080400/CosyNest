namespace System.DataFrancis;

/// <summary>
/// 这个记录可以为<see cref="IPushAttachmentChange{Obj}"/>提供附件被更改的信息
/// </summary>
/// <inheritdoc cref="IPushAttachmentChange{Obj}"/>
public sealed record PushAttachmentChangeInfo<Obj>
    where Obj : class, IPushAttachmentChange<Obj>
{
    #region 应当删除的附件
    /// <summary>
    /// 获取应该删除的附件
    /// </summary>
    public required IReadOnlyCollection<AttachmentDeleteInfo<Obj>> Delete { get; init; }
    #endregion
    #region 应当添加的附件
    /// <summary>
    /// 获取应该添加的附件
    /// </summary>
    public required IReadOnlyCollection<AttachmentAddInfo<Obj>> Add { get; init; }
    #endregion
    #region 更改完毕后的附件状态
    /// <summary>
    /// 获取更改完毕后的附件状态
    /// </summary>
    public required IReadOnlyCollection<Obj> AfterChange { get; init; }
    #endregion
    #region 是否发生更改
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示已经发生更改，存在添加或删除附件的情况
    /// </summary>
    public bool Changed
        => (Delete.Count, Add.Count) is not (0, 0);
    #endregion
    #region 获取副作用
    /// <summary>
    /// 获取这个附件更改信息的副作用，
    /// 如果它没有发生任何更改，
    /// 则为<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IPushAttachmentChange{Obj}.PushAttachmentChange(PushAttachmentChangeInfo{Obj}, string?, IServiceProvider)"/>
    public Func<IServiceProvider, Task>? SideEffect(string? basePath = null)
        => Changed ?
        serviceProvider => Obj.PushAttachmentChange(this, basePath, serviceProvider) :
        null;
    #endregion
    #region 批量获取副作用
    /// <summary>
    /// 批量获取本对象和其他<see cref="IPushAttachmentChange{Obj}"/>的副作用，
    /// 它们会被作为一个管道组合到一起
    /// </summary>
    /// <param name="changeInfos">其他的<see cref="IPushAttachmentChange{Obj}"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="SideEffect(string?)"/>
    public Func<IServiceProvider, Task>? SideEffect(IEnumerable<PushAttachmentChangeInfo<Obj>> changeInfos, string? basePath = null)
    {
        var sideEffects = changeInfos.Prepend(this).Select(x => x.SideEffect(basePath)).ToArray();
        return sideEffects.Aggregate((Func<IServiceProvider, Task>?)null, (seed, sideEffect) => seed.SideEffect(sideEffect));
    }
    #endregion
}
