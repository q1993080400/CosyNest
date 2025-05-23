﻿using System.Collections.Immutable;

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
    /// 当用户看到末尾的时候，
    /// 会自动加载新的元素
    /// </summary>
    [Parameter]
    public RenderFragment<RenderVirtualizationEndInfo>? RenderEnd { get; set; }
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
    #region 用来给元素进行编号的对象
    /// <summary>
    /// 这个对象用来给每个元素进行编号，
    /// 如果为<see langword="null"/>，
    /// 则不进行这个操作
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IElementNumber ElementNumber { get; set; }
    #endregion
    #region 每次渲染增加的数量
    /// <summary>
    /// 当每次延迟渲染元素时，
    /// 它控制新增渲染元素的数量
    /// </summary>
    [Parameter]
    public int Plus { get; set; } = ToolServerInterface.PlusDefault;
    #endregion
    #region 是否倒序渲染
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则执行倒序渲染，它从容器的底部开始渲染，
    /// 从顶部增加新元素，需要数据源和渲染方配合
    /// </summary>
    [Parameter]
    public bool IsReverse { get; set; }
    #endregion
    #region 末尾元素的ID
    /// <summary>
    /// 获取末尾元素的ID，
    /// 当用户看到末尾元素的时候，会加载新的元素，
    /// 注意：末尾元素的位置随着<see cref="IsReverse"/>而变化
    /// </summary>
    [Parameter]
    public string EndID { get; set; } = CreateASP.JSObjectName();
    #endregion
    #region 对偶元素的ID
    /// <summary>
    /// 获取对偶元素的ID，
    /// 对偶元素是和末尾元素成双成对的一个元素，
    /// 它们分别位于容器的两端
    /// </summary>
    [Parameter]
    public string PairingID { get; set; } = CreateASP.JSObjectName();
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
            DotNetObject.Dispose();
            if (RenderElements is { })
                await RenderElements.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
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
    private DotNetObjectReference<Virtualization<Element>> DotNetObject { get; }
    #endregion
    #region 旧的异步迭代器对象
    /// <summary>
    /// 获取旧的异步迭代器对象
    /// </summary>
    private IAsyncEnumerable<Element>? OldElements { get; set; }
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
    #region 是否需要跳跃到最后元素
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示需要跳跃到上一次渲染元素的末尾位置，
    /// 它只在倒序渲染中需要
    /// </summary>
    private bool NeedJumpLastElement { get; set; }
    #endregion
    #region 上一次渲染的元素数量
    /// <summary>
    /// 获取上一次渲染的元素数量，
    /// 它只在倒序渲染中需要
    /// </summary>
    private int LastElementListCount { get; set; }
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
            return await TryAddElement();
        }
        finally
        {
            Lock.Release();
        }
    }
    #endregion
    #region 尝试添加元素
    /// <summary>
    /// 尝试添加待渲染的元素，
    /// 并返回是否存在待渲染元素，
    /// 注意：这个方法没有加锁，可能存在线程安全问题
    /// </summary>
    /// <returns></returns>
    private async Task<bool> TryAddElement()
    {
        var oldIsComplete = IsComplete;
        #region 本地函数
        async Task<bool> Fun()
        {
            (var element, IsComplete) = await RenderElements.MoveRange(Plus);
            var newElement = element.ToArray();
            if (newElement.Length is 0)
            {
                LastElementListCount = 0;
                NeedJumpLastElement = false;
                await RenderElements.DisposeAsync();
                return false;
            }
            if (IsReverse)
                LastElementListCount = ElementList.Count;
            ElementList = IsReverse ?
                ElementList.InsertRange(0, newElement) :
                ElementList.AddRange(newElement);
            return true;
        }
        #endregion
        var needRender = await Fun();
        return needRender || (IsComplete != oldIsComplete);
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
    #region 新增元素并渲染
    /// <summary>
    /// 新增并渲染元素，
    /// 这个方法是专门给JS调用的，
    /// 不要在Net中调用它
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public Task AddElementAndRender()
        => InvokeAsync(async () =>
        {
            if (IsDispose || IsComplete)
                return;
            var needRedner = await NeedRender();
            if (!needRedner)
                return;
            if (IsReverse)
            {
                if (await JSWindow.InvokeAsync<bool>("CheckIntersecting", PairingID))
                    AnchoringReverse = true;
                else
                    NeedJumpLastElement = true;
            }
            StateHasChanged();
        });
    #endregion
    #region 是否锚定倒序渲染
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示为了配合倒序渲染，需要将容器滚动到最下方
    /// </summary>
    private bool AnchoringReverse { get; set; }
    #endregion
    #endregion
    #region 重写的方法
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsync()
    {
        var hasNewElements = Elements is null || !ReferenceEquals(OldElements, Elements);
        if (!hasNewElements)
            return;
        await Lock.WaitAsync();
        try
        {
            if (RenderElements is { })
                await RenderElements.DisposeAsync();
            RenderElements = (Elements ?? Array.Empty<Element>().ToAsyncEnumerable()).GetAsyncEnumerator();
            IsComplete = false;
            ElementList = [];
            OldElements = Elements;
            AnchoringReverse = IsReverse;
            LastElementListCount = 0;
            NeedJumpLastElement = false;
            await TryAddElement();
        }
        finally
        {
            Lock.Release();
        }
    }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsDispose)
            return;
        if (firstRender)
        {
            await JSWindow.InvokeVoidAsync("ObservingVirtualizationContainers", DotNetObject, nameof(AddElementAndRender), EndID);
            return;
        }
        #region 用来跳转到末尾的扩展方法
        async Task ScrollIntoViewToEnd()
        {
            if (!IsReverse || ElementList.Count is 0)
                return;
            if (AnchoringReverse)
            {
                AnchoringReverse = false;
                await JSWindow.InvokeVoidAsync("ScrollIntoViewToEnd", PairingID);
                return;
            }
            if (NeedJumpLastElement)
            {
                var lastElementListCountCache = LastElementListCount;
                LastElementListCount = 0;
                NeedJumpLastElement = false;
                await ElementNumber.JumpToElement(ElementList.Count - lastElementListCountCache, false, false);
            }
        }
        #endregion
        await ScrollIntoViewToEnd();
        if (IsComplete)
            return;
        if (await JSWindow.InvokeAsync<bool>("CheckIntersecting", EndID) && await NeedRender())
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
        var enumerableState = (Elements, IsComplete) switch
        {
            (null, _) => VirtualizationEnumerableState.NotEnumerable,
            ({ }, false) => VirtualizationEnumerableState.InEnumerable,
            ({ }, true) => ElementList.Count switch
            {
                > 0 => VirtualizationEnumerableState.Complete,
                _ => VirtualizationEnumerableState.Empty,
            },
        };
        var renderEndInfo = new RenderVirtualizationEndInfo()
        {
            EndID = EndID,
            EnumerableState = enumerableState
        };
        var renderEnd = RenderEnd is null ?
            x =>
        {
            x.OpenElement(0, "div");
            x.SetKey(EndID);
            x.AddAttribute(1, "id", EndID);
            x.CloseElement();
        }
        : RenderEnd(renderEndInfo);
        #region 转换渲染参数的方法
        IReadOnlyCollection<RenderVirtualizationElementInfo<Element>> GenerateRenderElementInfo(IEnumerable<Element> elements)
            => [.. elements.Select
            ((element, index) => new RenderVirtualizationElementInfo<Element>()
            {
                ComponentID = ElementNumber.GetElementID(index),
                Index = index,
                RenderElement = element
            })];
        #endregion
        return new()
        {
            RenderElements = ElementList,
            GenerateRenderElementInfo = GenerateRenderElementInfo,
            RenderLoadingPoint = renderEnd,
            RenderEndInfo = renderEndInfo,
            PairingID = PairingID
        };
    }
    #endregion
    #endregion
    #region 构造函数
    public Virtualization()
    {
        DotNetObject = DotNetObjectReference.Create(this);
    }
    #endregion
}
