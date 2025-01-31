using System.NetFrancis;

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
    protected IReadOnlySet<string>? Highlight { get; set; }
    #endregion
    #region 搜索视图状态
    /// <summary>
    /// 获取搜索视图状态，
    /// 如果将它赋值给<see cref="SearchPanel.SearchViewerState"/>参数，
    /// 就可以通过<see cref="RefreshSearch"/>显式刷新搜索
    /// </summary>
    protected SearchViewerState SearchViewerState { get; } = new();
    #endregion
    #region 依赖注入的强类型调用工厂
    /// <summary>
    /// 获取依赖注入的强类型调用工厂
    /// </summary>
    [Inject]
    protected IStrongTypeInvokeFactory StrongTypeInvokeFactory { get; set; }
    #endregion
    #region 提交搜索
    /// <summary>
    /// 提交搜索
    /// </summary>
    /// <param name="info">用来执行搜索的参数</param>
    /// <returns></returns>
    protected abstract Task SubmitSearch(SearchPanelSubmitInfo info);
    #endregion
    #region 刷新搜索
    /// <summary>
    /// 显式刷新搜索，
    /// 要正确地使用这个功能，
    /// 必须使用<see cref="SearchViewerState"/>作为搜索视图状态
    /// </summary>
    /// <returns></returns>
    protected async Task RefreshSearch()
    {
        var info = new SearchPanelSubmitInfo()
        {
            DataFilterDescription = SearchViewerState.GenerateFilter()
        };
        await SubmitSearch(info);
        this.StateHasChanged();
    }
    #endregion
}
