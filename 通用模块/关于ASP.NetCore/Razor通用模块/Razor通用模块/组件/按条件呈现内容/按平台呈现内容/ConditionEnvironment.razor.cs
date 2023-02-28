namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件允许根据不同平台呈现不同的内容，
/// 平台信息通过JS互操作获得，
/// 它还会提供一个<see cref="IEnvironmentInfoWeb"/>类型的级联参数供后代使用
/// </summary>
public sealed partial class ConditionEnvironment : ConditionPresent<IEnvironmentInfoWeb?>
{
    #region 依赖注入的JS对象
    [Inject]
    private IJSWindow JSWindow { get; set; }
    #endregion
    #region 获取平台信息
    private IEnvironmentInfoWeb? GetConditionField;

    protected override IEnvironmentInfoWeb? GetCondition => GetConditionField;
    #endregion
    #region 重写的OnAfterRenderAsync方法
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            GetConditionField = await JSWindow.Navigator.EnvironmentInfo();
            StateHasChanged();
        }
    }
    #endregion
}
