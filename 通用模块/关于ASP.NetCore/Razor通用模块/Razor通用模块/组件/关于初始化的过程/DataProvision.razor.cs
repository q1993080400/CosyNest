namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以提供异步遍历数据的迭代器，
/// 以及高亮和搜索功能，
/// 它可以和虚拟化组件以及搜索组件配合使用
/// </summary>
/// <typeparam name="Data">数据的类型</typeparam>
public abstract partial class DataProvision<Data> : ComponentBase
    where Data : class
{
    #region 所有数据
    /// <summary>
    /// 这个异步迭代器枚举所有数据
    /// </summary>
    protected IAsyncEnumerable<Data>? Datas { get; set; }
    #endregion
    #region 高亮文本
    /// <summary>
    /// 获取应该高亮的文本
    /// </summary>
    protected IReadOnlyCollection<string>? Highlight { get; set; }
    #endregion
    #region 提交搜索
    /// <summary>
    /// 提交搜索
    /// </summary>
    /// <param name="info">用来执行搜索的参数</param>
    /// <returns></returns>
    protected abstract Task SubmitSearch(SearchPanelSubmitInfo info);
    #endregion
}
