namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="ModelDialogClickSwitch"/>的参数，
/// 它同时会被作为级联参数传递下去
/// </summary>
public sealed record RenderModelDialogClickSwitchInfo
{
    #region 弹窗的状态
    /// <summary>
    /// 获取是否开启弹窗
    /// </summary>
    public required bool IsOpen { get; init; }
    #endregion
    #region 切换弹窗
    /// <summary>
    /// 这个委托可以切换弹窗状态
    /// </summary>
    public required Action Switch { get; init; }
    #endregion
    #region 关闭弹窗
    /// <summary>
    /// 这个委托可以用来关闭弹窗
    /// </summary>
    public required Action Close { get; init; }
    #endregion
}
