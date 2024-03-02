namespace System.Threading.Tasks;

/// <summary>
/// 这个类型可以用来报告进度
/// </summary>
public sealed record Progress
{
    #region 总体数量
    /// <summary>
    /// 获取总体数量
    /// </summary>
    public required decimal TotalCount { get; init; }
    #endregion
    #region 已完成数量
    /// <summary>
    /// 获取已完成数量
    /// </summary>
    public required decimal CompletedCount { get; init; }
    #endregion
    #region 已完成进度
    /// <summary>
    /// 获取已完成的进度
    /// </summary>
    public decimal CompletedProgress => Math.Min(1, CompletedCount / TotalCount);
    #endregion
    #region 是否完成
    /// <summary>
    /// 是否完成这个任务
    /// </summary>
    public bool IsCompleted => CompletedCount >= TotalCount;
    #endregion
    #region 计算总体进度
    /// <summary>
    /// 假设这个进度的每一个<see cref="CompletedCount"/>都代表一个阶段，
    /// 然后参数<paramref name="sonProgress"/>代表子阶段的进度，
    /// 则返回一个<see cref="Progress"/>，它表示总体进度，
    /// 通过这个方法，可以递归地计算总体进度
    /// </summary>
    /// <param name="sonProgress">子阶段的进度</param>
    /// <returns></returns>
    public Progress TotalProgress(Progress sonProgress)
        => this with
        {
            CompletedCount = this.CompletedCount + Math.Min(1, sonProgress.CompletedProgress)
        };
    #endregion
}
