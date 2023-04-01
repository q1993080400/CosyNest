namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件支持分页功能
/// </summary>
/// <typeparam name="Element">组件呈现的元素类型</typeparam>
public sealed partial class Pagination<Element> : ComponentBase
{
    #region 组件参数
    #region 当前页数
    /// <summary>
    /// 获取或设置当前页数，从0开始
    /// </summary>
    [Parameter]
    public int PageIndex { get; set; }
    #endregion
    #region 每页数量
    /// <summary>
    /// 获取每一页的元素数量
    /// </summary>
    [EditorRequired]
    [Parameter]
    public int PageSize { get; set; }
    #endregion
    #region 分页函数
    /// <summary>
    /// 这个委托的第一个参数是当前页数，
    /// 第二个参数是每一页的元素数量，返回值就是当前分页的元素
    /// </summary>
    [EditorRequired]
    [Parameter]
    public Func<int, int, Task<IEnumerable<Element>>> PageFunction { get; set; }
    #endregion
    #region 用于呈现元素的委托
    /// <summary>
    /// 获取用于呈现每个元素的委托，
    /// 它的参数就是待呈现的元素
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<Element> RenderFragment { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 该参数展开控制父div容器的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 当前页
    /// <summary>
    /// 获取或设置当前页的元素
    /// </summary>
    private IEnumerable<Element> Page { get; set; }
    #endregion
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsync()
    {
        Page = await PageFunction(PageIndex, PageSize);
    }
    #endregion
    #endregion
}
