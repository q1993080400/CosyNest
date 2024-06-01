using System.NetFrancis.Api;

namespace System.DingDing;

/// <summary>
/// 这个记录是专用于钉钉API的WebAPI返回值封装，
/// 它不携带任何额外内容
/// </summary>
public sealed record APIPackDingDing : APIPack, IHasAuthorizationDingDingState
{
    #region 钉钉身份验证状态
    public required AuthorizationDingDingState AuthorizationState { get; init; }
    #endregion 
}
