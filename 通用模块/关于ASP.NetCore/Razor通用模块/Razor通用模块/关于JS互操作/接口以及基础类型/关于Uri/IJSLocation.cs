namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个JS中Location对象的Net封装，
/// 它表示当前窗口的Uri
/// </summary>
public interface IJSLocation
{
    #region 关于Uri
    #region 获取或设置当前Uri
    /// <summary>
    /// 通过这个异步属性，
    /// 可以获取或设置当前Uri，
    /// 在写入这个属性时，可以写入相对或绝对Uri
    /// </summary>
    IAsyncProperty<string> Href { get; }
    #endregion
    #region 获取主机名称
    /// <summary>
    /// 获取主机名称和端口号
    /// </summary>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask<string> Host(CancellationToken cancellation = default);
    #endregion
    #region 获取源部分
    /// <summary>
    /// 获取源部分，它包括协议，主机和端口
    /// </summary>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask<string> Origin(CancellationToken cancellation = default);
    #endregion
    #region 获取协议部分
    /// <summary>
    /// 获取Uri的协议部分
    /// </summary>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask<string> Protocol(CancellationToken cancellation = default);
    #endregion
    #region 获取或设置锚部分
    /// <summary>
    /// 通过这个异步属性，
    /// 可以获取或设置当前Uri的锚部分，
    /// 它可以用来导航到页面中的一部分
    /// </summary>
    IAsyncProperty<string> Hash { get; }
    #endregion
    #endregion
    #region 刷新页面
    /// <summary>
    /// 刷新当前页面
    /// </summary>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask Reload(CancellationToken cancellation = default);
    #endregion
}
