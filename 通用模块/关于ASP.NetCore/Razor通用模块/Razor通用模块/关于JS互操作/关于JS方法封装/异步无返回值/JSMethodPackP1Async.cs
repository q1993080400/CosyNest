namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型可以用来简化在JS中调用拥有一个参数的异步方法的过程
/// </summary>
/// <typeparam name="T1">第一个参数的类型</typeparam>
/// <inheritdoc cref="JSMethodPack{Delegate, NetPack}.JSMethodPack(Delegate)"/>
sealed class JSMethodPackAsync<T1>(Func<T1, Task> @delegate) : JSMethodPack<Func<T1, Task>, JSMethodPackAsync<T1>>(@delegate)
{
    #region 调用方法
    /// <summary>
    /// 调用这个委托封装
    /// </summary>
    /// <param name="t1">委托的第一个参数</param>
    /// <returns></returns>
    [JSInvokable]
    public Task Invoke(T1 t1)
        => PackDelegate(t1);
    #endregion
    #region 内部成员：获取Net封装引用的方法
    protected override DotNetObjectReference<JSMethodPackAsync<T1>> CreateDotNetObjectReference()
        => JSInterop.DotNetObjectReference.Create(this);
    #endregion
}
