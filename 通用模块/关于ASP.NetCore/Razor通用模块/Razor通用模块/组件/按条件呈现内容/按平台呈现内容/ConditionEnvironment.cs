using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件允许根据不同平台呈现不同的内容，
/// 平台信息通过JS互操作获得
/// </summary>
public sealed class ConditionEnvironment : ConditionPresent<IEnvironmentInfoWeb?>
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
