namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录可以用来封装一个WebAPI的返回值
/// </summary>
/// <typeparam name="Return">WebAPI的返回值类型</typeparam>
public sealed record APIReturnPack<Return> : APIPack
{
    #region API的返回值
    /// <summary>
    /// 获取这个WebAPI的返回值，
    /// 如果失败，则为默认值
    /// </summary>
    public Return? Value { get; init; }
    #endregion
}
