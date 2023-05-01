using System.Text.Json;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本组件可以用来实现虚拟化加载，
/// 当滚动到末尾时加载新的内容
/// </summary>
/// <typeparam name="Element">元素的类型</typeparam>
public sealed partial class Virtualization<Element> : Component, IAsyncDisposable
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
    #region 枚举元素的迭代器
#pragma warning disable BL0007
    private IAsyncEnumerator<Element> ElementsField;

    /// <summary>
    /// 获取枚举元素的迭代器
    /// </summary>
    [EditorRequired]
    [Parameter]
    public IAsyncEnumerator<Element> Elements
    {
        get => ElementsField;
        set
        {
            AgainRender = !ReferenceEquals(ElementsField, value);
            if ((AgainRender, ElementsField) is (true, { } e))
                Task.Run(e.DisposeAsync);
            ElementsField = value;
        }
    }
#pragma warning restore
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
    #region 跳跃到指定位置
    /// <summary>
    /// 在第一次呈现页面后，会滚动到这个位置，
    /// 它可以用于记忆上次的滚动位置
    /// </summary>
    [Parameter]
    public int? Jump { get; set; }
    #endregion
    #region 是否为倒序容器
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该容器为倒序容器，它初始滚动到容器最下方，
    /// 当滚动到容器最上方时，延迟加载元素
    /// </summary>
    [Parameter]
    public bool IsReverse { get; set; }

    /*注意：
      当这个属性为false时，建议对Elements也进行优化，
      先返回后面的元素*/
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        await Elements.DisposeAsync();
        NetPackReference?.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 关于渲染
    #region 是否重新渲染
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示<see cref="Elements"/>已经发生改变，
    /// 需要先将<see cref="Rendered"/>清空，再进行渲染
    /// </summary>
    private bool AgainRender { get; set; }
    #endregion
    #region 已渲染的元素
    /// <summary>
    /// 获取已经渲染的元素
    /// </summary>
    private ICollection<Element> Rendered { get; set; } = new LinkedList<Element>();
    #endregion
    #region 简化渲染
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它可以用来渲染单个元素
    /// </summary>
    /// <param name="render">渲染的参数</param>
    /// <returns></returns>
    private RenderFragment SimplifyRender(RenderVirtualizationElementInfo<Element> render)
        => RenderElement(render);
    #endregion
    #region 是否初始化完毕
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示组件初始化已经完毕，第一批元素已经尝试加载
    /// </summary>
    private bool Initializationed { get; set; }
    #endregion
    #region 容器元素ID
    /// <summary>
    /// 获取容器元素的ID
    /// </summary>
    private string ID { get; } = ToolASP.CreateJSObjectName().ToString();
    #endregion
    #region 已渲染数量
    /// <summary>
    /// 获取已渲染元素的数量
    /// </summary>
    private int RenderedCount => Rendered.Count;
    #endregion
    #region 根据索引生成元素ID
    /// <summary>
    /// 根据元素的索引，生成一个元素ID，
    /// 它让JS可以找到生成的每个元素
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string GetElementID(int index)
        => $"{ID}-{index}";
    #endregion
    #region 关于新增渲染
    #region OnScroll事件触发时执行的脚本
    /// <summary>
    /// 返回当OnScroll事件触发时，执行的脚本
    /// </summary>
    /// <returns></returns>
    private string OnScroll()
        => $$"""
        if({{NotRender}})
            return;
        var element=document.getElementById('{{ID}}');
        if(element==null)
            return;
        var scrollTop=element.scrollTop;
        var height=element.scrollHeight;
        var top=scrollTop+{{(IsReverse ? "0" : "element.clientHeight")}};
        var scrollPercentage=top / height;
        var needRender=scrollPercentage{{(IsReverse ? "<0.01" : ">0.99")}};
        if(needRender&&!{{NotRender}})
        {
            {{NotRender}}=true;
            {{OnAddRenderJSMethodName}}(0);
        }
        """;
    #endregion
    #region 当需要新增渲染元素时触发的事件
    /// <summary>
    /// 当需要新增渲染元素时，触发的事件
    /// </summary>
    /// <returns></returns>
    private async void OnAddRender()
    {
        var newElement = (await Elements.MoveRange(Plus)).Element;
        if (newElement.Any())
        {
            Rendered.Add(newElement);
            if (IsReverse)
            {
                var element = await JSWindow.Document.GetElementById(ID);
                if (element is { })
                    await element.ScrollFromPercentage(0, 0.02);
            }
            StateHasChanged();
        }
        await JSWindow.InvokeCodeVoidAsync($"{NotRender}=false");
    }
    #endregion
    #region 新增渲染的JS方法名称
    /// <summary>
    /// 新增渲染的JS方法名称，
    /// 它封装了<see cref="OnAddRender"/>方法
    /// </summary>
    private string OnAddRenderJSMethodName { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region Net对象封装引用
    /// <summary>
    /// 获取Net对象封装引用，
    /// 它将<see cref="OnAddRender"/>方法注册到JS运行时中
    /// </summary>
    private IDisposable? NetPackReference { get; set; }
    #endregion
    #region 不允许渲染的变量的表达式
    /// <summary>
    /// 获取一个JS表达式，它指向一个布尔类型的变量，
    /// 当这个变量为<see langword="true"/>时，
    /// 不允许执行<see cref="OnAddRender"/>方法
    /// </summary>
    private const string NotRender = "window.notRender";
    #endregion
    #endregion
    #endregion 
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsyncRealize()
    {
        if (AgainRender)
        {
            await GoTop();
            Rendered.Clear();
            Rendered.Add((await Elements.MoveRange(Initial)).Element);
            Initializationed = true;
        }
    }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsyncRealize(bool firstCompletelyRender)
    {
        if (!firstCompletelyRender)
            return;
        await JSWindow.InvokeCodeVoidAsync($"{NotRender}=false");
        (_, NetPackReference) = await JSWindow.Document.PackNetMethod((JsonElement _) => OnAddRender(), OnAddRenderJSMethodName);
        await Task.Delay(ToolASP.BaseTimeOut * 2);
        var div = await JSWindow.Document.GetElementById(ID) ??
            throw new NullReferenceException($"未能找到ID为{ID}的元素，{nameof(Virtualization<Element>)}不能正常工作");
        while (await div.ScrollTop is 0 && await div.ScrollPercentage(false) >= 0.98)
        {
            //这是为了防止由于Initial过低，初始加载完全部的元素后仍然不显示滚动条，导致无法滚动加载剩余元素的问题
            var newElement = (await Elements.MoveRange(Plus)).Element;
            if (newElement.Any())
            {
                Rendered.Add(newElement);
            }
            else break;
            StateHasChanged();
            await Task.Delay(ToolASP.BaseTimeOut * 1.5);
            if (IsReverse)
                await div.ScrollFromPercentage(0, 1);
        }
        if (Jump is { } i)
            await JSWindow.Document.ScrollIntoView(GetElementID(i));
    }
    #endregion
    #region 关于跳跃到顶端
    #region 跳跃到最顶端的方法
    /// <summary>
    /// 调用这个方法可以跳跃到组件最顶端或最底端，
    /// 视<see cref="IsReverse"/>属性的值来决定
    /// </summary>
    /// <returns></returns>
    private async Task GoTop()
    {
        var container = await JSWindow.Document.GetElementById(ID);
        if (container is { })
            await container.ScrollFromPercentage(0, IsReverse ? 1 : 0, true);
    }
    #endregion
    #endregion
    #region 获取用来渲染组件的参数
    /// <summary>
    /// 获取用来渲染组件的参数
    /// </summary>
    /// <returns></returns>
    private RenderVirtualizationInfo<Element> GetRenderInfo()
    {
        var rendered = IsReverse ? Rendered.Reverse() : Rendered;
        var renderElement = rendered.PackIndex().Select(x => new RenderVirtualizationElementInfo<Element>()
        {
            Element = x.Elements,
            Index = x.Index,
            ID = GetElementID(x.Index)
        }).Select(SimplifyRender).ToArray();
        return new()
        {
            RenderElement = renderElement,
            RenderContainer = new()
            {
                CSS = "virtualizationContainer",
                ID = ID,
                OnScrollScript = OnScroll(),
                GoTop = new(this, GoTop)
            }
        };
    }
    #endregion
    #endregion
}
