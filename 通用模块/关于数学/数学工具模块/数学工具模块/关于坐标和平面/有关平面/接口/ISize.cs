using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来描述一个二维平面的范围
/// </summary>
/// <typeparam name="Num">用来描述平面的数字类型</typeparam>
public interface ISize<Num> : IEquatable<ISize<Num>>
    where Num : INumber<Num>
{
    #region 返回平面的宽度和高度
    #region 说明文档
    /*实现本API请遵循以下规范：
      #Width和Height不允许负值，
      如果在构造时传入负值，请抛出异常
    
      #当Num是整数和浮点数时，这个属性具有不同的含义：
      如果Num是整数，它表示水平和垂直方向的像素数量，
      如果Num是浮点数，它表示平面最大和最小的XY坐标的差，举个例子：
    
      假设本属性是(1,1)，它表示这个平面有且只有一个像素点，
      如果平面的开始坐标是(0,0)，那么它的结束坐标也是(0,0)，
      假设本属性是(1.0,1.0)，它表示这个平面长和宽为1，
      如果平面的开始坐标为(0.0,0.0)，那么它的结束坐标应该是(1.0,1.0)*/
    #endregion
    #region 宽度
    /// <summary>
    /// 获取平面的宽度
    /// </summary>
    Num Width { get; }
    #endregion
    #region 高度
    /// <summary>
    /// 获取平面的高度
    /// </summary>
    Num Height { get; }
    #endregion
    #endregion
    #region 解构ISize
    /// <summary>
    /// 将这个平面大小解构为宽和高
    /// </summary>
    /// <param name="width">平面的宽</param>
    /// <param name="height">平面的高</param>
    void Deconstruct(out Num width, out Num height)
    {
        width = Width;
        height = Height;
    }
    #endregion
}
