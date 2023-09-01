using System.Collections.Immutable;
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
    #region 渲染空集合时的委托
    /// <summary>
    /// 获取一个渲染空集合时的委托
    /// </summary>
    [Parameter]
    public RenderFragment RenderEmpty { get; set; } = _ => { };
    #endregion
    #region 枚举元素的迭代器
    #region 正式属性
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
            Old = ElementsField;
            ElementsField = value;
        }
    }
#pragma warning restore
    #endregion
    #region 旧迭代器缓存
    /// <summary>
    /// 缓存旧的迭代器，
    /// 它可以用来比较旧迭代器和新迭代器是否是同一个迭代器，
    /// 从而得出是否需要重置<see cref="Rendered"/>
    /// </summary>
    private IAsyncEnumerator<Element>? Old { get; set; }
    #endregion  
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
    #region 是否准备好进行渲染了
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示已经渲染完初始元素，可以开始渲染了
    /// </summary>
    private bool Ready { get; set; }
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
    #region 留白高度
    /// <summary>
    /// 获取留白高度的样式文本，
    /// 它在容器最下方提供一个空白高度，
    /// 这可以使容器变得更加美观
    /// </summary>
    [Parameter]
    public string BlankHeight { get; set; } = "margin-top:15dvh;";
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        ContinueRendering = false;
        if (Old is { })
            await Old.DisposeAsync();
        await Elements.DisposeAsync();
        NetPackReference?.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 关于渲染
    #region 已渲染的元素
    /// <summary>
    /// 获取已经渲染的元素
    /// </summary>
    private ImmutableList<Element> Rendered { get; set; } = ImmutableList<Element>.Empty;
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
            Rendered = Rendered.AddRange(newElement);
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
    #region 是否应继续渲染
    /// <summary>
    /// 如果这个值为<see langword="false"/>，
    /// 表示组件已经被释放，应该停止一切渲染
    /// </summary>
    private bool ContinueRendering { get; set; } = true;
    #endregion
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsyncRealize(bool firstCompletelyRender)
    {
        if (State == RenderState.Render3 && (Old, ReferenceEquals(Old, Elements)) is ({ }, false))
        {
            await Old.DisposeAsync();
            Old = null;
            Rendered = ImmutableList<Element>.Empty;
            await RefreshContainer();
            return;
        }
        if (!firstCompletelyRender)
            return;
        (_, NetPackReference) = await JSWindow.Document.PackNetMethod((JsonElement _) => OnAddRender(), OnAddRenderJSMethodName);
        await RefreshContainer();
        if (Jump is { } i)
            await JSWindow.Document.ScrollIntoView(GetElementID(i));
    }
    #endregion
    #region 刷新容器
    /// <summary>
    /// 刷新虚拟化容器，重新渲染所有元素
    /// </summary>
    /// <returns></returns>
    private async Task RefreshContainer()
    {
        try
        {
            var div = await JSWindow.Document.GetElementById(ID) ??
            throw new NullReferenceException($"未能找到ID为{ID}的元素，{nameof(Virtualization<Element>)}不能正常工作");
            await JSWindow.InvokeCodeVoidAsync($"{NotRender}=false");
            while (ContinueRendering)
            {
                var top = await div.ScrollTop;
                var percentage = await div.ScrollPercentage(false, true);
                if ((top, percentage) is not (0, >= 0.98) && Rendered.Count > 0)
                    break;
                //这是为了防止由于Initial过低，初始加载完全部的元素后仍然不显示滚动条，导致无法滚动加载剩余元素的问题
                var newElement = (await Elements.MoveRange(Ready ? Plus : Initial)).Element;
                Ready = true;
                Rendered = Rendered.AddRange(newElement);
                StateHasChanged();
                await Task.Delay(ToolASP.BaseTimeOut * 1.5);
                if (IsReverse)
                    await div.ScrollFromPercentage(0, 1);
                if (!newElement.Any())
                    break;
            }
        }
        catch (JSDisconnectedException)
        {

        }
        catch (JSException ex)
        {
            ex.Log(ServiceProvider);
        }
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
        var renderElement = rendered.PackIndex().Select(x =>
        {
            #region 本地函数
            void Delete()
            => Rendered = Rendered.Remove(x.Elements);
            #endregion
            return new RenderVirtualizationElementInfo<Element>()
            {
                Element = x.Elements,
                Index = x.Index,
                ID = GetElementID(x.Index),
                DeleteElement = new(this, Delete)
            };
        }).
        Select(x => RenderElement(x)).ToArray();
        return new()
        {
            RenderElement = renderElement,
            RenderContainer = new()
            {
                CSS = "virtualizationContainer",
                ID = ID,
                OnScrollScript = OnScroll(),
                GoTop = new(this, GoTop),
                BlankHeight = BlankHeight,
                RenderEmpty = RenderEmpty,
                State = renderElement.Length switch
                {
                    not 0 => RenderVirtualizationState.HasElements,
                    0 => Ready ? RenderVirtualizationState.NotElements : RenderVirtualizationState.NotReady
                }
            }
        };
    }
    #endregion
    #endregion
}
