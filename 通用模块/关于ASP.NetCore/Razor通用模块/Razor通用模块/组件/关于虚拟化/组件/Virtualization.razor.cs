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
    /// 获取渲染末尾的委托，
    /// 它的参数是末尾元素的ID，
    /// 必须将这个ID赋值给这个元素，
    /// 并给它加上Key，
    /// 当用户看到末尾的时候，自动加载新的元素
    /// </summary>
    [Parameter]
    public RenderFragment<string>? RenderEnd { get; set; }
    #endregion
    #region 枚举元素的迭代器
    /// <summary>
    /// 获取枚举元素的迭代器，
    /// 警告：如果它和以前的迭代器是两个不同的引用，
    /// 那么就会重新进行迭代，如果为<see langword="null"/>，
    /// 则不加载任何元素
    /// </summary>
    [EditorRequired]
    [Parameter]
    public IAsyncEnumerable<Element>? Elements { get; set; }
    #endregion
    #region 每次渲染增加的数量
    /// <summary>
    /// 当每次延迟渲染元素时，
    /// 它控制新增渲染元素的数量
    /// </summary>
    [Parameter]
    public int Plus { get; set; } = 15;
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    #region 正式方法
    public async ValueTask DisposeAsync()
    {
        IsDispose = true;
        await Lock.WaitAsync();
        try
        {
            if (RenderElements is { })
                await RenderElements.DisposeAsync();
            PackNet?.Dispose();
        }
        finally
        {
            Lock.Release();
            Lock.Dispose();
        }
    }
    #endregion
    #region 指示是否已经释放
    /// <summary>
    /// 指示这个组件是否已经被释放
    /// </summary>
    private bool IsDispose { get; set; }
    #endregion
    #region 封送到JS的Net对象
    /// <summary>
    /// 获取封送到JS的Net对象
    /// </summary>
    private IDisposable? PackNet { get; set; }
    #endregion
    #region JS方法的名称
    /// <summary>
    /// 获取封装的JS方法的名称
    /// </summary>
    private string? JSMethodName { get; } = CreateASP.JSObjectName();
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 关于要渲染的元素
    #region 要渲染的元素列表
    /// <summary>
    /// 获取要渲染的元素列表
    /// </summary>
    private ImmutableList<Element> ElementList { get; set; } = [];
    #endregion
    #region 异步迭代器
    /// <summary>
    /// 获取用来枚举元素的异步迭代器
    /// </summary>
    private IAsyncEnumerator<Element> RenderElements { get; set; }
    #endregion
    #region 锁对象
    /// <summary>
    /// 获取锁对象
    /// </summary>
    private SemaphoreSlim Lock { get; } = new(1);
    #endregion
    #region 是否需要继续渲染
    /// <summary>
    /// 尝试向集合添加元素，
    /// 并返回是否需要继续渲染
    /// </summary>
    /// <returns></returns>
    private async Task<bool> NeedRender()
    {
        if (IsComplete || IsDispose)
            return false;
        await Lock.WaitAsync();
        try
        {
            var oldIsComplete = IsComplete;
            #region 本地函数
            async Task<bool> Fun()
            {
                (var element, IsComplete) = await RenderElements.MoveRange(Plus);
                var newElement = element.ToArray();
                if (newElement.Length is 0)
                {
                    await RenderElements.DisposeAsync();
                    return false;
                }
                ElementList = ElementList.AddRange(newElement);
                return true;
            }
            #endregion
            var needRender = await Fun();
            return needRender || (IsComplete != oldIsComplete);
        }
        finally
        {
            Lock.Release();
        }
    }
    #endregion
    #region 集合是否已枚举完毕
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示集合已经枚举完毕
    /// </summary>
    private bool IsComplete { get; set; }
    #endregion
    #endregion
    #region 关于渲染
    #region 末尾元素的ID
    /// <summary>
    /// 获取末尾元素的ID
    /// </summary>
    private string EndID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 新增元素并渲染
    /// <summary>
    /// 新增并渲染元素
    /// </summary>
    /// <returns></returns>
    private Task AddElementAndRender()
        => InvokeAsync(async () =>
        {
            if (IsDispose || IsComplete)
                return;
            var needRedner = await NeedRender();
            if (needRedner)
                StateHasChanged();
        });
    #endregion
    #endregion
    #region 重写的方法
    #region 重写SetParametersAsync
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var dictionary = parameters.ToDictionary().ToDictionary();
        var elements = parameters.GetValueOrDefault<IAsyncEnumerable<Element>>(nameof(Elements));
        var hasNewElements = elements is null || !ReferenceEquals(elements, Elements);
        if (hasNewElements)
        {
            await Lock.WaitAsync();
            try
            {
                if (RenderElements is { })
                    await RenderElements.DisposeAsync();
                RenderElements = (elements ?? Array.Empty<Element>().ToAsyncEnumerable()).GetAsyncEnumerator();
                IsComplete = elements is null;
                ElementList = [];
            }
            finally
            {
                Lock.Release();
            }
        }
        await base.SetParametersAsync(ParameterView.FromDictionary(dictionary));
        PackNet ??= (await JSWindow.Document.PackNetMethod<JsonElement>(_ => AddElementAndRender(), JSMethodName)).Freed;
        if (hasNewElements)
        {
            await JSWindow.InvokeCodeVoidAsync($"""
                ObservingVirtualizationContainers({JSMethodName},'{EndID}');
                """);
        }
    }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsComplete || IsDispose)
            return;
        await Task.Delay(1000);
        var intersecting = await JSWindow.InvokeAsync<bool>("CheckIntersecting", EndID);
        if (intersecting && await NeedRender())
            StateHasChanged();
    }
    #endregion
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderVirtualizationInfo<Element> GetRenderInfo()
    {
        var renderEnd = RenderEnd is null ?
            x =>
        {
            x.OpenElement(0, "div");
            x.SetKey(EndID);
            x.AddAttribute(1, "id", EndID);
            x.CloseElement();
        }
        : RenderEnd(EndID);
        return new()
        {
            DataSource = ElementList,
            RenderLoadingPoint = renderEnd,
            EnumerableState = (Elements, IsComplete) switch
            {
                (null, _) => VirtualizationEnumerableState.NotEnumerable,
                ({ }, false) => VirtualizationEnumerableState.InEnumerable,
                ({ }, true) => ElementList.Count switch
                {
                    > 0 => VirtualizationEnumerableState.Complete,
                    _ => VirtualizationEnumerableState.Empty,
                },
            }
        };
    }
    #endregion
    #endregion
}
