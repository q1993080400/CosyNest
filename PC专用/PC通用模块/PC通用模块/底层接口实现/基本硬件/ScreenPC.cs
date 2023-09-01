using System.Maths;
using System.Maths.Plane;
using System.Runtime.InteropServices;

namespace System.Underlying.PC;

/// <summary>
/// 这个类型是<see cref="IScreen"/>的实现，
/// 封装了当前PC硬件屏幕的信息
/// </summary>
sealed partial class ScreenPC : IScreen
{
    #region 获取物理分辨率
    public ISizePixel Resolution { get; }
    #endregion
    #region 逻辑分辨率
    public ISizePixel LogicalResolution => Resolution;
    #endregion
    #region 获取DPI
    #region X轴DPI
    public int DPIX { get; }
    #endregion
    #region Y轴DPI
    public int DPIY { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 无参数构造函数
    /// </summary>
    public ScreenPC()
    {
        DPIX = DpiX();
        DPIY = DpiY();
        Resolution = DESKTOP();
    }
    #endregion
    #region Win32API调用
    [LibraryImport("user32.dll")]
    private static partial IntPtr GetDC(IntPtr ptr);

    [LibraryImport("gdi32.dll")]
    private static partial int GetDeviceCaps(IntPtr hdc, int nIndex);

    [LibraryImport("user32.dll", EntryPoint = "ReleaseDC")]
    private static partial IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

    const int LOGPIXELSX = 88;

    const int LOGPIXELSY = 90;

    const int DESKTOPVERTRES = 117;

    const int DESKTOPHORZRES = 118;

    private static int DpiX()
    {
        var hdc = GetDC(IntPtr.Zero);
        var dpiX = GetDeviceCaps(hdc, LOGPIXELSX);
        ReleaseDC(IntPtr.Zero, hdc); return dpiX;
    }

    private static int DpiY()
    {
        var hdc = GetDC(IntPtr.Zero);
        var dpiX = GetDeviceCaps(hdc, LOGPIXELSY);
        ReleaseDC(IntPtr.Zero, hdc); return dpiX;
    }

    /// <summary>
    /// 获取真实设置的桌面分辨率大小
    /// </summary>
    private static ISizePixel DESKTOP()
    {
        var hdc = GetDC(IntPtr.Zero);
        var size = CreateMath.SizePixel(
            GetDeviceCaps(hdc, DESKTOPHORZRES),
            GetDeviceCaps(hdc, DESKTOPVERTRES));
        ReleaseDC(IntPtr.Zero, hdc);
        return size;
    }
    #endregion
}
