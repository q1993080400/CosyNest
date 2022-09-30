using System.Maths.Plane;

namespace System.Underlying.PC;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个鼠标
/// </summary>
public interface IMouse : IPointer
{
    #region 左键双击
    /// <summary>
    /// 左键双击
    /// </summary>
    /// <param name="point">要左键双击的绝对坐标</param>
    void ClickDouble(IPoint point)
    {
        Click(point);
        Click(point);
    }
    #endregion
    #region 右键单击
    /// <summary>
    /// 右键单击
    /// </summary>
    /// <param name="point">要右键单击的绝对坐标</param>
    void ClickRight(IPoint point);
    #endregion
    #region 滚动鼠标
    /// <summary>
    /// 滚动鼠标滚轮
    /// </summary>
    /// <param name="amplitude">滚动幅度，
    /// 正数代表向上滚动，负数代表向下滚动</param>
    void Scroll(int amplitude);
    #endregion
}
