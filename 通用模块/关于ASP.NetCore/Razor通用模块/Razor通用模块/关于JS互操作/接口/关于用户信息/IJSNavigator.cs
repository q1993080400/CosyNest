using System.Underlying;

using Microsoft.AspNetCore.Http;

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
    #region 获取或设置剪贴板文本
    /// <summary>
    /// 获取或设置剪贴板文本
    /// </summary>
    IAsyncProperty<string?> ClipboardText { get; }
    #endregion
    #region 获取定位对象
    /// <summary>
    /// 获取一个可以用来定位的对象
    /// </summary>
    IPosition Geolocation { get; }
    #endregion
}
