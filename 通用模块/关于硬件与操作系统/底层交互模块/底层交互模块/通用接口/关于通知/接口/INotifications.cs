namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来发送通知
/// </summary>
/// <typeparam name="Parameter">用来发送通知的参数的类型</typeparam>
public interface INotifications<in Parameter>
{
    #region 发送通知
    /// <summary>
    /// 发送一条通知
    /// </summary>
    /// <param name="parameter">用来发送通知的参数</param>
    /// <param name="cancellationToken">一个用来取消异步操作的对象</param>
    /// <returns></returns>
    Task Send(Parameter parameter, CancellationToken cancellationToken = default);
    #endregion
}
