namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来引发事件
/// </summary>
public interface IEventInvoke
{
    #region 动态调用事件
    /// <summary>
    /// 动态调用事件中的所有委托
    /// </summary>
    /// <param name="parameters">方法调用的参数列表</param>
    void Invoke(params object?[] parameters);
    #endregion
    #region 异步动态调用事件
    /// <inheritdoc cref="Invoke(object?[])"/>
    Task InvokeAsync(params object?[] parameters);
    #endregion
}
