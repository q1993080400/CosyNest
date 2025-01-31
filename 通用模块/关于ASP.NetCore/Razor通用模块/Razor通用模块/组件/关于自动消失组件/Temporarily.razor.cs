namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许显示一个内容，
/// 然后在一段时间后自动消失
/// </summary>
public sealed partial class Temporarily : ComponentBase, IContentComponent<RenderFragment>, IDisposable
{
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 寿命
    /// <summary>
    /// 获取这个组件的寿命，
    /// 在寿命结束的时候，它就会消失
    /// </summary>
    [Parameter]
    [EditorRequired]
    public TimeSpan Lifetime { get; set; }
    #endregion
    #region 消失后执行的委托
    /// <summary>
    /// 获取组件消失后执行的委托
    /// </summary>
    [Parameter]
    public Func<Task>? OnLifeEnd { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region 释放对象
    public void Dispose()
        => IsDispose = true;
    #endregion
    #endregion
    #region 内部成员
    #region 是否隐藏
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该隐藏，否则应该显示
    /// </summary>
    private bool IsHide { get; set; }
    #endregion
    #region 是否被释放
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个组件已经被释放
    /// </summary>
    private bool IsDispose { get; set; }
    #endregion
    #region 重写OnAfterRenderAsync方法
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsHide)
            return;
        await Task.Delay(Lifetime);
        if (IsDispose)
            return;
        IsHide = true;
        if (OnLifeEnd is { })
            await OnLifeEnd();
        this.StateHasChanged();
    }
    #endregion
    #endregion
}
