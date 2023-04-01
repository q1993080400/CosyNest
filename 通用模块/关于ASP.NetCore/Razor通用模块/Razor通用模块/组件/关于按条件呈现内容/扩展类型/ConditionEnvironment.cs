namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件允许根据不同平台呈现不同的内容，
/// 平台信息通过JS互操作获得，
/// 它还会提供一个<see cref="IEnvironmentInfoWeb"/>类型的级联参数供后代使用
/// </summary>
public sealed class ConditionEnvironment : ConditionJS<IEnvironmentInfoWeb>
{
    #region 返回呈现条件
    protected override async Task<IEnvironmentInfoWeb> GetConditionFromJS(IJSWindow jsWindow)
        => await jsWindow.Navigator.EnvironmentInfo();
    #endregion
    #region 返回呈现条件是否固定
    protected override bool IsFixed() => true;
    #endregion
}
