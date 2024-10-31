using System.MathFrancis;

namespace Microsoft.JSInterop;

/// <summary>
/// 本类型是通过JS实现的屏幕对象
/// </summary>
sealed class JSScreen : IJSScreen
{
    #region 设备像素比
    public required double DevicePixelRatio { get; init; }
    #endregion
    #region 物理分辨率
    public ISize<int> Resolution => throw new NotImplementedException();
    #endregion
    #region 逻辑分辨率
    public required ISize<double> LogicalResolution { get; init; }
    #endregion
    #region X轴DPI
    public int DPIX => throw new NotImplementedException();
    #endregion
    #region Y轴DPI
    public int DPIY => throw new NotImplementedException();
    #endregion
}
