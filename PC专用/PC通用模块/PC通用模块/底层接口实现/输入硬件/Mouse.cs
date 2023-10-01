using System.MathFrancis;
using System.MathFrancis.Plane;
using System.Runtime.InteropServices;

namespace System.Underlying.PC;

/// <summary>
/// 该类型是<see cref="IMouse"/>的实现，
/// 可以视为一个鼠标
/// </summary>
sealed class Mouse : IMouse
{
#pragma warning disable CA1806

    #region 底层实现支持
    #region Win32结构
    [StructLayout(LayoutKind.Sequential)]
    private struct POINT(int x, int y)
    {
        public int X = x;
        public int Y = y;
    }
    #endregion
    #region Win32方法封装
    /// <summary>
    /// 操作鼠标的Win32方法
    /// </summary>
    /// <param name="dwFlags">指示鼠标事件的类型</param>
    /// <param name="dx">鼠标的X坐标</param>
    /// <param name="dy">鼠标的Y坐标</param>
    /// <param name="dwData">如果事件类型为滚动滚轮，则它指示滚动的幅度</param>
    /// <param name="dwExtraInfo">该参数意义不明</param>
    /// <returns></returns>
    [DllImport("user32")]
    private static extern int mouse_event(int dwFlags, int dx, int dy, int dwData = 0, int dwExtraInfo = 0);
    #endregion
    #region Win32枚举
    /// <summary>
    /// 模拟鼠标左键按下 
    /// </summary>
    const int MOUSEEVENTF_LEFTDOWN = 0x0002;

    /// <summary>
    /// 模拟鼠标左键抬起 
    /// </summary>
    const int MOUSEEVENTF_LEFTUP = 0x0004;

    /// <summary>
    /// 模拟鼠标右键按下 
    /// </summary>
    const int MOUSEEVENTF_RIGHTDOWN = 0x0008;

    /// <summary>
    /// 模拟鼠标右键抬起
    /// </summary>
    const int MOUSEEVENTF_RIGHTUP = 0x0010;

    /// <summary>
    /// 标示采用绝对坐标 
    /// </summary>
    const int MOUSEEVENTF_ABSOLUTE = 0x8000;

    /// <summary>
    /// 模拟鼠标滚轮滚动操作，必须配合dwData参数
    /// </summary>
    const int MOUSEEVENTF_WHEEL = 0x0800;
    #endregion
    #endregion
    #region 鼠标位置
    public IPoint Pos
    {
        get
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern bool GetCursorPos(out POINT pt);

            GetCursorPos(out var pi);
            return CreateMath.Point(pi.X, -pi.Y);
        }
    }
    #endregion
    #region 点击鼠标
    public void Click(IPoint point)
        => mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, point.Right, -point.Top);
    #endregion
    #region 右键单击
    public void ClickRight(IPoint point)
        => mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, point.Right, -point.Top);
    #endregion
    #region 滚动鼠标
    public void Scroll(int amplitude)
        => mouse_event(MOUSEEVENTF_WHEEL, 0, 0, amplitude);
    #endregion
}
