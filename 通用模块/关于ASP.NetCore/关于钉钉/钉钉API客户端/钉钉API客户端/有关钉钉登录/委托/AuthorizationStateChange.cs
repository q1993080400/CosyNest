namespace Microsoft.AspNetCore;

/// <summary>
/// 当钉钉授权状态改变时，触发这个委托
/// </summary>
/// <param name="newState">新的授权状态，
/// 如果为<see langword="null"/>，表示退出登录</param>

public delegate Task AuthorizationStateChange(AuthorizationDingDingState? newState);
