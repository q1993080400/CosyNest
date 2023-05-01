using System.Maths.Plane;

namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个指针输入设备
/// </summary>
public interface IPointer
{
    #region 说明文档
    /*问：什么是指针输入设备？
      答：指的是通过二维平面位置进行输入的设备，
      例如PC平台上的鼠标，手机平台上的手指等等*/
    #endregion
    #region 获取指针位置
    /// <summary>
    /// 获取指针当前的位置
    /// </summary>
    IPoint Pos { get; }
    #endregion
    #region 点击指针
    /// <summary>
    /// 点击指针
    /// </summary>
    /// <param name="point">要点击的绝对坐标</param>
    void Click(IPoint point);
    #endregion
}
