using System.NetFrancis.Api;

namespace System.DingDing;

/// <summary>
/// 这个记录是专用于钉钉API的WebAPI返回值封装，
/// 它可以额外携带一个内容对象
/// </summary>
/// <inheritdoc cref="APIReturnPack{Return}"/>
public sealed record APIReturnPackDingDing<Return> : APIReturnPack<Return>, IHasAuthorizationDingDingState
{
    #region 钉钉身份验证状态
    public required AuthorizationDingDingState AuthorizationState { get; init; }
    #endregion 
}
