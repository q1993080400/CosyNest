using System.Reflection.Metadata;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许进行钉钉身份验证，
/// 注意：由于身份验证实际由钉钉负责，
/// 因此它不是AspNetCore身份验证体系的一部分
/// </summary>
public sealed partial class AuthorizeViewDingDing : ComponentBase
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
    #region 级联参数：身份验证状态
    /// <summary>
    /// 获取身份验证状态，如果为<see langword="null"/>，
    /// 表示未进行身份验证，或者身份验证未通过
    /// </summary>
    [CascadingParameter]
    private AuthenticationDingDingState? AuthenticationState { get; set; }
    #endregion
    #region 级联参数：钉钉App信息
    /// <summary>
    /// 获取钉钉App信息
    /// </summary>
    [CascadingParameter]
    private DingDingAppInfo? AppInfo { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 是否授权通过
    /// <summary>
    /// 获取是否授权通过
    /// </summary>
    private bool AuthorizationPassed { get; set; }
    #endregion
    #region 获取回调地址
    /// <summary>
    /// 获取登录的回调地址
    /// </summary>
    private string RedirectUri
        => UriManager.Uri.UriHost!.ToString();
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (AuthenticationState is { })
            return;
        #region 用于更新组件的本地函数
        async Task Update(AuthenticationDingDingRequest request)
        {
            var state = (await HttpClient.Request<IGetAuthenticationDingDingState, APIPackDingDing>
                    (x => x.GetAuthenticationDingDingState(request))).AuthorizationState;
            await AuthorizationStateChange(state);
        }
        #endregion
        var request = await JSWindow.GetAuthenticationRequest();
        switch ((request, AuthCode, AppInfo))
        {
            case (null, null, { }):
                var loginPageUri = new UriComplete("https://login.dingtalk.com/oauth2/auth")
                {
                    UriParameter = new(
                    [
                        ("redirect_uri",RedirectUri),
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
                AppInfo = await HttpClient.Request<IGetAuthenticationDingDingState, DingDingAppInfo>(x => x.GetAppInfo());
                this.StateHasChanged();
                break;
            case ({ }, _, _):
                await Update(request);
                break;
            case (null, { }, _):
                var parameter = new AuthenticationDingDingRequest()
                {
                    Code = AuthCode,
                    IsToken = false,
                    RefreshToken = null,
                };
                await Update(parameter);
                break;
        }
    }
    #endregion
    #region 当授权状态改变时触发的事件
    /// <inheritdoc cref="AspNetCore.AuthorizationStateChange"/>
    private async Task AuthorizationStateChange(AuthorizationDingDingState? newState)
    {
        AuthenticationState = newState?.AuthenticationState;
        AuthorizationPassed = newState?.AuthorizationPassed ?? false;
        await newState.PersistenceAuthorizationState(JSWindow);
        this.StateHasChanged();
    }
    #endregion
    #region 注销钉钉账号
    /// <summary>
    /// 注销钉钉账号，退出登录
    /// </summary>
    /// <returns></returns>
    private async Task LogOut()
    {
        await AuthorizationStateChange(null);
    }
    #endregion
    #endregion
}
