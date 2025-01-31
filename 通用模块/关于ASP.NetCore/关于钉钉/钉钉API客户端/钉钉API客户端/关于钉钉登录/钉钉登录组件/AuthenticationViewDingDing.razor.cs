using System.Reflection.Metadata;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许进行钉钉身份验证，
/// 注意：由于身份验证实际由钉钉负责，
/// 因此它不是AspNetCore身份验证体系的一部分
/// </summary>
public sealed partial class AuthenticationViewDingDing : ComponentBase
{
    #region 组件参数
    #region 通过身份验证后的渲染方式
    /// <summary>
    /// 通过身份验证后的渲染方式
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<AuthenticationDingDingState> Authorized { get; set; }
    #endregion
    #region 未通过身份验证后的渲染方式
    /// <summary>
    /// 未通过身份验证后的渲染方式，它的参数是一个委托，
    /// 通过它可以注销钉钉账号，重新登陆
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<Func<Task>> NotAuthorized { get; set; }
    #endregion
    #region 路由参数：身份验证代码
    /// <summary>
    /// 这个参数用于接收路由身份验证代码
    /// </summary>
    [SupplyParameterFromQuery]
    private string? AuthCode { get; set; }
    #endregion
    #endregion
    #region 静态成员：用于注销的级联参数名称
    /// <summary>
    /// 获取一个键，它用来作为一个级联参数的名称，
    /// 这个级联参数可以用来注销钉钉身份验证
    /// </summary>
    public const string LogOutParameterKey = "208E6AEE-C54B-AAD2-1480-BBB7842559AA";
    #endregion
    #region 内部成员
    #region 身份验证状态
    /// <summary>
    /// 获取身份验证状态，如果为<see langword="null"/>，
    /// 表示未进行身份验证，或者身份验证未通过
    /// </summary>
    private AuthenticationDingDingState? AuthenticationState { get; set; }
    #endregion
    #region 钉钉App信息
    /// <summary>
    /// 获取钉钉App信息
    /// </summary>
    private DingDingAppInfo? AppInfo { get; set; }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (AuthenticationState is { })
            return;
        #region 用于更新组件的本地函数
        async Task Update(AuthenticationDingDingRequest request)
        {
            var state = (await StrongTypeInvokeFactory.StrongType<IGetAuthenticationDingDingState>().
                Invoke(x => x.GetAuthenticationDingDingState(request))).AuthenticationState;
            await AuthenticationStateChange(state);
        }
        #endregion
        var result = await JSWindow.GetAuthenticationResult();
        switch ((result, AuthCode, AppInfo))
        {
            case (null, null, { }):
                var loginPageUri = new UriComplete("https://login.dingtalk.com/oauth2/auth")
                {
                    UriParameter = new(
                    [
                        ("redirect_uri",HostProvide.Host),
                        ("response_type","code"),
                        ("client_id",AppInfo!.ClientID),
                        ("scope","openid corpid"),
                        ("prompt","consent"),
                        ("org_type",""),
                        ("corpId",AppInfo.OrganizationID)
                    ])
                };
                NavigationManager.NavigateTo(loginPageUri);
                break;
            case (null, null, null):
                AppInfo = await StrongTypeInvokeFactory.StrongType<IGetAuthenticationDingDingState>().
                    Invoke(x => x.GetAppInfo());
                StateHasChanged();
                break;
            case ({ }, _, _):
                await Update(result.GetAuthenticationRequest());
                break;
            case (null, { }, _):
                var parameter = new AuthenticationDingDingRequest()
                {
                    Code = AuthCode,
                    IsToken = false,
                    RefreshToken = null,
                    IsEncryption = false
                };
                await Update(parameter);
                break;
        }
    }
    #endregion
    #region 当身份验证状态改变时触发的事件
    /// <summary>
    /// 当身份验证状态改变时触发的事件
    /// </summary>
    /// <param name="newState">新授权状态</param>
    /// <returns></returns>
    private async Task AuthenticationStateChange(AuthenticationDingDingState? newState)
    {
        AuthenticationState = newState;
        await newState.PersistenceAuthenticationState(JSWindow);
        StateHasChanged();
    }
    #endregion
    #region 注销钉钉账号
    /// <summary>
    /// 注销钉钉账号，退出登录
    /// </summary>
    /// <returns></returns>
    private async Task LogOut()
    {
        await AuthenticationStateChange(null);
    }
    #endregion
    #endregion
}
