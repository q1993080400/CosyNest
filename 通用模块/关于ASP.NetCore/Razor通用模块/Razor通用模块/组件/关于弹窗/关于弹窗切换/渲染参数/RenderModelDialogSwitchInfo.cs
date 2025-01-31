namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="ModelDialogSwitch"/>的参数
/// </summary>
public sealed record RenderModelDialogSwitchInfo
{
    #region 是否打开
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表弹窗处于打开状态，否则代表处于关闭状态
    /// </summary>
    public required bool IsOpen { get; init; }
    #endregion
    #region 切换后的新状态
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表调用<see cref="SwitchOpen"/>以后，
    /// 弹窗处于打开状态，否则处于关闭状态
    /// </summary>
    public bool SwitchOpenState
        => !IsOpen;
    #endregion
    #region 用来切换打开状态的委托
    /// <summary>
    /// 获取用来切换打开状态的委托，
    /// 它可以用来打开或关闭弹窗
    /// </summary>
    public required Action SwitchOpen { get; init; }
    #endregion
}
