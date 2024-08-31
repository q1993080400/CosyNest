using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录可以作为一个搜索参数，
/// 它可以向后端发起搜索请求
/// </summary>
public sealed record WebSearchInfo
{
    #region 索引
    /// <summary>
    /// 获取元素的页索引
    /// </summary>
    public required int Index { get; init; }
    #endregion
    #region 筛选条件
    /// <summary>
    /// 获取筛选这些元素的条件
    /// </summary>
    public required DataFilterDescription FilterCondition { get; init; }
    #endregion
}
