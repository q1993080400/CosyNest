using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个JS中Screen对象的Net封装，
/// 它可以获取有关当前屏幕的信息
/// </summary>
public interface IJSScreen : IScreen
{
    #region 说明文档
    /*实现本API请遵循以下规范：
      #分辨率，物理大小等请返回浏览器可用的空间，
      因为剩下的空间对于前端开发而言毫无意义*/
    #endregion
    #region 返回设备像素比
    /// <summary>
    /// 返回本设备的设备像素比
    /// </summary>
    double DevicePixelRatio { get; }
    #endregion
}
