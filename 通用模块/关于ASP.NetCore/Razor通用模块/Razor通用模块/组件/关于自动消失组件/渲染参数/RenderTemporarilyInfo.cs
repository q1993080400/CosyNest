namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="Temporarily"/>的参数
/// </summary>
public sealed record RenderTemporarilyInfo
{
    #region 立即使其消失的委托
    /// <summary>
    /// 调用这个委托可以使其立即消失
    /// </summary>
    public required Func<Task> DisappearImmediately { get; init; }
    #endregion
}
