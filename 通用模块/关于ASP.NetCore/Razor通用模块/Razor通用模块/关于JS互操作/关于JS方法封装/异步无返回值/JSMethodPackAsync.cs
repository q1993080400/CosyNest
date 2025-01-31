namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型可以用来简化在JS中调用无参数异步方法的过程
/// </summary>
/// <inheritdoc cref="JSMethodPack{Delegate, NetPack}.JSMethodPack(Delegate)"/>
sealed class JSMethodPackAsync(Func<Task> @delegate) : JSMethodPack<Func<Task>, JSMethodPackAsync>(@delegate)
{
    #region 调用方法
    /// <summary>
    /// 调用这个委托封装
    /// </summary>
    /// <returns></returns>
    [JSInvokable]
    public Task Invoke()
        => PackDelegate();
    #endregion
    #region 内部成员：获取Net封装引用的方法
    protected override DotNetObjectReference<JSMethodPackAsync> CreateDotNetObjectReference()
        => JSInterop.DotNetObjectReference.Create(this);
    #endregion
}
