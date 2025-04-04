﻿using System.MathFrancis;

namespace System.Underlying;

/// <summary>
/// 这个接口封装了有关当前硬件屏幕的信息
/// </summary>
public interface IScreen
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #移动设备的屏幕方向可以改变，
      在这种情况下，分辨率，物理大小，以及一切依赖于它们的属性按照当前屏幕方向计算，
      举例说明，假设某一手机在水平放置时分辨率(2400,1080)，
      则它在垂直放置时，分辨率应该返回(1080,2400)*/
    #endregion
    #region 物理分辨率
    /// <summary>
    /// 获取屏幕的物理分辨率
    /// </summary>
    ISize<int> Resolution { get; }
    #endregion
    #region 逻辑分辨率
    /// <summary>
    /// 获取屏幕的逻辑分辨率
    /// </summary>
    ISize<double> LogicalResolution { get; }
    #endregion
    #region 获取DPI
    #region X轴DPI
    /// <summary>
    /// 获取当前屏幕X轴的DPI
    /// </summary>
    int DPIX { get; }
    #endregion
    #region Y轴DPI
    /// <summary>
    /// 获取当前屏幕Y轴的DPI
    /// </summary>
    int DPIY { get; }
    #endregion
    #endregion
    #region 获取屏幕类型
    /// <summary>
    /// 获取这个屏幕的类型
    /// </summary>
    ScreenType ScreenType => LogicalResolution.Width switch
    {
        >= 1224 => ScreenType.PC,
        >= 667 => ScreenType.Tablet,
        _ => ScreenType.Phone
    };
    #endregion
}
