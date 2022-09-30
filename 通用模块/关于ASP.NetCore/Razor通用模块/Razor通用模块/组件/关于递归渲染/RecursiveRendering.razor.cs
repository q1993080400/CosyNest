namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是递归渲染组件的基类
/// </summary>
/// <typeparam name="P">递归渲染的参数</typeparam>
public abstract partial class RecursiveRendering<P> : ComponentBase
{
    #region 说明文档
    /*问：按照规范，应该多用组合，少用继承，
      在UI方面更是如此，那么为什么本类型被设计为递归渲染组件的基类，
      而不是一个递归渲染的容器？
      答：这是因为为了处理用户交互，需要保存组件的状态，
      而且递归渲染涉及很多层级，很难使用一个父组件保存所有的状态，
      因此作者决定，在这个问题上破一次例*/
    #endregion
    #region 组件参数
    #region 递归渲染参数
    /// <summary>
    /// 获取递归渲染的参数
    /// </summary>
    [EditorRequired]
    [Parameter]
    public P Parameter { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 递归子项
    /// <summary>
    /// 获取或设置递归子项，
    /// 它用来进行下一次递归
    /// </summary>
    protected IEnumerable<P>? Son { get; set; }
    #endregion
    #region 设置递归子项
    /// <summary>
    /// 调用这个方法初始化<see cref="Son"/>
    /// </summary>
    /// <returns></returns>
    protected abstract Task InitializationSon();
    #endregion
    #endregion
}
