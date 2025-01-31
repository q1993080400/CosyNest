
namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以初始化一个对象，
/// 在初始化完成之前，不显示任何内容
/// </summary>
/// <typeparam name="Interface">服务端接口的类型</typeparam>
/// <typeparam name="Parameter">用来搜索对象的类型，
/// 它应该包含实体类的ID，或本身就是ID</typeparam>
/// <typeparam name="Content">要寻找的对象的类型</typeparam>
public sealed partial class Initialization<Interface, Parameter, Content> : ComponentBase
    where Interface : class, IServerFind<Parameter, Content>
    where Content : class
{
    #region 组件参数
    #region 用来获取搜索参数的委托
    /// <summary>
    /// 用来获取搜索参数的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Task<Parameter>> GetFindParameter { get; set; }
    #endregion
    #region 找到对象后的渲染委托
    /// <summary>
    /// 当找到这个对象后的渲染委托，
    /// 它的参数就是找到的这个对象
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<Content> Find { get; set; }
    #endregion
    #region 没有找到对象时的渲染委托
    /// <summary>
    /// 没有找到对象时的渲染委托
    /// </summary>
    [Parameter]
    public RenderFragment? NotFind { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 要渲染的对象
    /// <summary>
    /// 获取要渲染的对象
    /// </summary>
    private Content? Obj { get; set; }
    #endregion
    #region 是否初始化完毕
    /// <summary>
    /// 指示初始化是否已完毕
    /// </summary>
    private bool InitializationComplete { get; set; }
    #endregion
    #region 搜索参数
    /// <summary>
    /// 获取搜索参数
    /// </summary>
    private Parameter? FindParameter { get; set; }
    #endregion
    #region 重写OnParametersSetAsync
    protected override async Task OnParametersSetAsync()
    {
        FindParameter = await GetFindParameter();
        Obj = await StrongTypeInvokeFactory.StrongType<Interface>().
            Invoke(x => x.Find(FindParameter));
        InitializationComplete = true;
    }
    #endregion
    #endregion
}
