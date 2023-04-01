namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件允许根据不同屏幕呈现不同的内容，
/// 屏幕信息通过JS互操作获得，
/// 它还会提供一个<see cref="IJSScreen"/>类型的级联参数供后代使用
/// </summary>
public sealed class ConditionScreen : ConditionJS<IJSScreen>
{
    #region 获取呈现条件
    protected override async Task<IJSScreen> GetConditionFromJS(IJSWindow jsWindow)
        => await jsWindow.Screen;
    #endregion
    #region 返回呈现条件是否固定
    protected override bool IsFixed() => true;
    #endregion
}
