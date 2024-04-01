using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为JS中Navigator对象的Net封装，
/// 通过它可以在权限范围内和底层硬件打交道
/// </summary>
public interface IJSNavigator
{
    #region 获取基本硬件信息
    /// <summary>
    /// 获取当前硬件的基本信息
    /// </summary>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<IEnvironmentInfoWeb> EnvironmentInfo(CancellationToken cancellation = default);
    #endregion
    #region 剪切板对象
    /// <summary>
    /// 获取一个可以用来读写剪切板的对象
    /// </summary>
    IJSClipboard Clipboard { get; }
    #endregion
    #region 获取定位对象
    /// <summary>
    /// 获取一个可以用来定位的对象
    /// </summary>
    IPosition Geolocation { get; }
    #endregion
    #region 获取唤醒锁
    /// <summary>
    /// 获取一个唤醒锁，
    /// 它可以阻止屏幕变暗，
    /// 当释放它的时候，唤醒锁自动解除，
    /// 如果返回<see langword="null"/>，表示获取失败
    /// </summary>
    /// <returns></returns>
    Task<IAsyncDisposable?> GetWakeLock();
    #endregion
}
