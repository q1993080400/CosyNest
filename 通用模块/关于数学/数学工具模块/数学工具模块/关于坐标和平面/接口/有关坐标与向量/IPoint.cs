using static System.Maths.CreateBaseMath;

namespace System.Maths.Plane;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个二维平面坐标
/// </summary>
public interface IPoint : IEquatable<IPoint>
{
    #region 返回坐标原点
    /// <summary>
    /// 返回坐标原点，也就是(0,0)
    /// </summary>
    public static IPoint Original { get; }
    = new Point(0, 0);
    #endregion
    #region 坐标的位置
    #region 说明文档
    /*问：为什么要用Right和Top，
      而不用传统的X和Y来表示坐标的位置？

      答：按照数学上的习惯，y越大代表坐标越靠近顶部，
      但是按照UI开发中的习惯，y越小代表坐标越靠近顶部，
      这造成了歧义，而且很容易产生Bug，
      因此设计者决定，使用含义更加明确的Right和Top来表示坐标的位置
      如果a.Top>b.Top，那么a一定在b的上方

      问：既然如此，如何区分数学坐标和UI坐标？
      答：按照约定，只要是外界可以访问到的IPoint对象，
      包括方法的返回值，那么它一定是数学坐标，
      即便这个模块和UI有关，也应当遵循这个原则，
      如果底层模块只识别UI坐标，那么转换应该在后台隐式进行，
      不能让调用者察觉到两者的区别*/
    #endregion
    #region 水平位置
    /// <summary>
    /// 这个值越大，代表该坐标越靠近右边
    /// </summary>
    Num Right { get; }
    #endregion
    #region 垂直位置
    /// <summary>
    /// 这个值越大，代表该坐标越靠近顶部
    /// </summary>
    Num Top { get; }
    #endregion
    #endregion
    #region 转换坐标
    #region 返回镜像坐标
    /// <summary>
    /// 返回本坐标的镜像坐标
    /// </summary>
    /// <param name="mirrorHorizontal">如果这个值为<see langword="true"/>，则镜像X轴</param>
    /// <param name="mirrorVertical">如果这个值为<see langword="true"/>，则镜像Y轴</param>
    /// <returns></returns>
    IPoint Mirror(bool mirrorHorizontal, bool mirrorVertical)
        => CreateMath.Point(mirrorHorizontal ? -Right : Right, mirrorVertical ? -Top : Top);
    #endregion
    #region 移动坐标
    /// <summary>
    /// 移动这个坐标，并返回一个新坐标
    /// </summary>
    /// <param name="right">向右方向的水平偏移</param>
    /// <param name="top">向上方向的垂直偏移，如果为<see langword="null"/>，则数值和<see cref="Right"/>相同</param>
    /// <returns></returns>
    IPoint Move(Num right, Num? top = null)
        => CreateMath.Point(Right + right, Top + top ?? right);
    #endregion
    #region 转换为相对坐标
    /// <summary>
    /// 返回这个点相对于另一个点的相对坐标
    /// </summary>
    /// <param name="original">相对坐标的原点</param>
    /// <returns></returns>
    IPoint ToRel(IPoint original)
    {
        var (or, ot) = original;
        return Move(-or, -ot);
    }
    #endregion
    #region 转换为绝对坐标
    /// <summary>
    ///如果本坐标是相对于原点O的相对坐标，
    ///求本坐标的绝对坐标
    /// </summary>
    /// <param name="o">绝对坐标O，以它为原点，如果为<see langword="null"/>，则为(0,0)</param>
    /// <returns></returns>
    IPoint ToAbs(IPoint? o = null)
    {
        var (or, ot) = o ?? Original;
        return Move(or, ot);
    }
    #endregion
    #region 转换为极坐标
    /// <summary>
    /// 将本直角坐标转换为极坐标
    /// </summary>
    /// <param name="original">极坐标的原点，默认为(0,0)</param>
    /// <returns></returns>
    IVector ToPC(IPoint? original = default)
    {
        var (r, t) = ToRel(original ?? Original);
        var p = Math.Sqrt(r * r + t * t);               //计算出极坐标的极径
        var o = Math.Atan2(t, r);
        return new Vector(p, Unit(o, IUTAngle.Radian));
    }
    #endregion
    #endregion
    #region 解构坐标
    /// <summary>
    /// 将本坐标解构为水平位置和垂直位置
    /// </summary>
    /// <param name="right">坐标的水平位置，越大代表越靠近右边</param>
    /// <param name="top">坐标的垂直位置，越大代表越靠近顶部</param>
    void Deconstruct(out Num right, out Num top)
    {
        right = Right;
        top = Top;
    }
    #endregion
}
