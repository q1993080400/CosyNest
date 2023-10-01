using System.Collections.Immutable;
using System.Text.Json;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本组件可以用来实现虚拟化加载，
/// 当滚动到末尾时加载新的内容
/// </summary>
/// <typeparam name="Element">元素的类型</typeparam>
public sealed partial class Virtualization<Element> : ComponentBase, IAsyncDisposable
{
    #region 组件参数
    #region 渲染每个元素的委托
    /// <summary>
    /// 获取用来渲染每个元素的委托
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<RenderVirtualizationElementInfo<Element>> RenderElement { get; set; }
    #endregion
    #region 渲染组件的委托
    /// <summary>
    /// 获取用来渲染组件的委托
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<RenderVirtualizationInfo<Element>> RenderComponent { get; set; }
    #endregion
    #region 渲染末尾的委托
    /// <summary>
    /// 获取用来渲染末尾的委托，
    /// 它的参数就是末尾元素的ID，
    /// 当用户滚动到末尾元素时，
    /// 会延迟加载新的元素
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<string> RenderEnd { get; set; }
    #endregion
    #region 渲染空集合时的委托
    /// <summary>
    /// 获取一个渲染空集合时的委托
    /// </summary>
    [Parameter]
    public RenderFragment RenderEmpty { get; set; } = _ => { };
    #endregion
    #region 枚举元素的迭代器
    /// <summary>
    /// 获取枚举元素的迭代器，
    /// 警告：如果它和以前的迭代器是两个不同的引用，
    /// 那么就会重新进行迭代
    /// </summary>
    [EditorRequired]
    [Parameter]
    public IAsyncEnumerable<Element> Elements { get; set; }
    #endregion
    #region 每次渲染增加的数量
    /// <summary>
    /// 当每次延迟渲染元素时，
    /// 它控制新增渲染元素的数量
    /// </summary>
    [Parameter]
    public int Plus { get; set; } = 5;
    #endregion
    #region 初始渲染的数量
    /// <summary>
    /// 初始渲染元素的数量
    /// </summary>
    [Parameter]
    public int Initial { get; set; } = 35;
    #endregion
    #region 是否为倒序容器
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该容器为倒序容器，它初始滚动到容器最下方，
    /// 当滚动到容器最上方时，延迟加载元素，
    /// 注意：这个功能需要数据源配合，它也应该倒着返回元素
    /// </summary>
    [Parameter]
    public bool IsReverse { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    #region 正式方法
    public async ValueTask DisposeAsync()
    {
        IsDispose = true;
        if (RenderElements is { })
            await RenderElements.DisposeAsync();
        PackNet?.Dispose();
    }
    #endregion
    #region 指示是否已经释放
    /// <summary>
    /// 指示这个组件是否已经被释放
    /// </summary>
    private bool IsDispose { get; set; }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 关于要渲染的元素
    #region 要渲染的元素列表
    /// <summary>
    /// 获取要渲染的元素列表
    /// </summary>
    private ImmutableList<Element> ElementList { get; set; } = ImmutableList<Element>.Empty;
    #endregion
    #region 枚举要渲染的元素
    /// <summary>
    /// 枚举要渲染的元素
    /// </summary>
    private IAsyncEnumerator<Element> RenderElements { get; set; }
    #endregion
    #region 向元素集合添加元素
    /// <summary>
    /// 向元素集合添加元素
    /// </summary>
    /// <param name="elementCount">要添加的元素的数量</param>
    /// <param name="isReverse">如果这个值<see langword="true"/>，
    /// 表示该容器为倒序容器，它和<see cref="IsReverse"/>属性不等同，
    /// 因为执行这个方法时，组件未必已经初始化</param>
    /// <returns>集合是否已经全部枚举完毕</returns>
    private async Task<bool> AddElement(int elementCount, bool isReverse)
    {
        var (element, toEnd) = await RenderElements.MoveRange(elementCount);
        var newElement = isReverse ? element.Reverse() : element;
        ElementList = isReverse ? ElementList.InsertRange(0, newElement) : ElementList.AddRange(newElement);
        return toEnd;
    }
    #endregion
    #endregion
    #region 关于渲染
    #region 容器ID
    /// <summary>
    /// 获取这个容器的ID
    /// </summary>
    private string ContainerID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 封送到JS的Net对象
    /// <summary>
    /// 获取封送到JS的Net对象
    /// </summary>
    private IDisposable? PackNet { get; set; }
    #endregion
    #region 末尾元素的ID
    /// <summary>
    /// 获取末尾元素的ID
    /// </summary>
    private string EndID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 容器底部元素的ID
    /// <summary>
    /// 获取容器底部元素的ID，
    /// 它和<see cref="EndID"/>是两个概念，
    /// 因为要考虑到倒序容器的可能性
    /// </summary>
    private string BottomID => EndID + "Bottom";
    #endregion
    #region 新增元素并渲染
    /// <summary>
    /// 新增并渲染元素
    /// </summary>
    /// <returns></returns>
    private Task AddElementAndRender()
        => this.InvokeAsync(async () =>
        {
            if (IsDispose)
                return;
            var count = ElementList.Count;
            await AddElement(Plus, IsReverse);
            if (!IsDispose && count != ElementList.Count)
                this.StateHasChanged();
        });
    #endregion
    #endregion
    #region 重写的方法
    #region 重写SetParametersAsync
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var elements = parameters.GetValueOrDefault<IAsyncEnumerable<Element>>(nameof(Elements)) ??
            throw new NullReferenceException($"必须指定组件的{nameof(Elements)}参数，它对组件的渲染非常重要");
        if (!ReferenceEquals(elements, Elements))
        {
            if (RenderElements is { })
                await RenderElements.DisposeAsync();
            RenderElements = elements.GetAsyncEnumerator();
            ElementList = ImmutableList<Element>.Empty;
            var isReverse = parameters.GetValueOrDefault<bool>(nameof(IsReverse));
            await AddElement(Initial, isReverse);
        }
        await base.SetParametersAsync(parameters);
    }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsReverse)
        {
            var lastElementindex = Math.Min(ElementList.Count, Plus) - 1;
            if (lastElementindex > 0)
                await JSWindow.Document.ScrollIntoView(GetElementID(lastElementindex));
        }
        if (!firstRender)
            return;
        if (IsReverse)
            await JSWindow.Document.ScrollIntoView(BottomID);
        (var methodName, PackNet) = await JSWindow.Document.PackNetMethod<JsonElement>(_ => AddElementAndRender());
        await JSWindow.InvokeCodeVoidAsync($"""
                ObservingVirtualizationContainers({methodName},'{EndID}');
                """);
    }
    #endregion
    #endregion
    #region 根据元素索引获取元素ID
    /// <summary>
    /// 根据元素索引，获取元素ID
    /// </summary>
    /// <param name="elementIndex">元素的索引</param>
    /// <returns></returns>
    private string GetElementID(int elementIndex)
        => ContainerID + elementIndex;
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderVirtualizationInfo<Element> GetRenderInfo()
    {
        var renderElementInfo = ElementList.Select((x, index) => new RenderVirtualizationElementInfo<Element>()
        {
            Element = x,
            Index = index,
            ID = GetElementID(index),
            Delete = new(this, () => ElementList = ElementList.RemoveAt(index))
        });
        var renderElement = renderElementInfo.Select(x => RenderElement(x)).Append(x =>
        {
            x.OpenElement(0, "div");
            x.AddAttribute(1, "id", BottomID);
            x.CloseElement();
        });
        var renderEnd = RenderEnd(EndID);
        var finalRenderElement = (IsReverse ? renderElement.Prepend(renderEnd) : renderElement.Append(renderEnd)).ToArray();
        return new()
        {
            AnyElement = !ElementList.IsEmpty,
            RenderEmpty = x =>
            {
                RenderEmpty(x);
                renderEnd(x);
            },
            RenderElement = finalRenderElement
        };
    }
    #endregion
    #endregion
}
