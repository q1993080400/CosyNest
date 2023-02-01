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
    /// 获取用来渲染每个元素的委托，
    /// 它的参数依次是待渲染的元素，
    /// 元素的索引，以及一个为该元素生成的唯一ID，
    /// 通过ID可以找到这个元素
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<(Element, int, string)> RenderElement { get; set; }
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
            if (ElementsField is { } e)
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
    #region 参数展开
    /// <summary>
    /// 该参数展开控制父div容器的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 容器元素ID
    /// <summary>
    /// 获取容器元素的ID
    /// </summary>
    public string ID { get; } = Guid.NewGuid().ToString();
    #endregion
    #region 已渲染数量
    /// <summary>
    /// 获取已渲染元素的数量
    /// </summary>
    public int RenderedCount => Rendered.Count;
    #endregion
    #region 根据索引生成元素ID
    /// <summary>
    /// 根据元素的索引，生成一个元素ID，
    /// 它让JS可以找到生成的每个元素
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetElementID(int index)
        => $"{ID}-{index}";
    #endregion
    #region 滚动时触发的事件
    /// <summary>
    /// 滚动容器div触发的事件
    /// </summary>
    /// <returns></returns>
    public async Task OnScroll()
    {
        var element = (await JSWindow.Document.GetElementById(ID))!;
        if (IsReverse ? (await element.ScrollPercentage(true) < 0.01) :
            (await element.ScrollPercentage(false) > 0.99))
        {
            var newElement = (await Elements.MoveRange(Plus)).Element;
            if (newElement.Any())
            {
                Rendered.Add(newElement);
                if (IsReverse)
                    await element.ScrollFromPercentage(0, 0.02);
            }
        }
    }

    /*问：为什么要公开这个方法？
      答：这是因为如果通过参数展开为容器div设置了Onscroll事件，
      则这个方法会被覆盖，无法调用，进一步导致虚拟化加载功能失效，
      因此需要将方法公开，使调用者能够在Onscroll事件中重新调用本方法*/
    #endregion
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        await Elements.DisposeAsync();
        GC.SuppressFinalize(this);
    }
    #endregion
    #endregion
    #region 内部成员
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
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsyncRealize()
    {
        if (AgainRender)
        {
            Rendered.Clear();
            Rendered.Add((await Elements.MoveRange(Initial)).Element);
        }
    }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsyncRealize(bool firstCompletelyRender)
    {
        if (!firstCompletelyRender)
            return;
        await Task.Delay(ToolASP.BaseTimeOut * 2);
        var div = (await JSWindow.Document.GetElementById(ID))!;
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
    #endregion
}
