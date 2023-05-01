namespace System.Maths.Plane;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以视为一个用极坐标表示的二维向量
/// </summary>
public interface IVector
{
    #region 说明文档
    /*说明：
      问：这个接口可以用来表示什么？
      答：可以表示以下数学概念：
      1.二维向量
      2.一个极坐标

      问：为什么要用一个类型表示两种对象？这违背了职责明确原则
      答：这两个概念的实现一致，都具有且只具有长度和方向，
      如果将它们拆成两个类型，会有大量重复，
      而且它们在数学上本身就是极其类似的

      另外在本类型的早期版本中，本类型还可以表示一条线段，
      但是现在这个定义已被删除，因为作者认识到：
      向量不仅是一个几何概念，也是一个物理概念，
      因此它实现IGeometric并不是一个良好的设计*/
    #endregion
    #region 向量的长度
    /// <summary>
    /// 获取向量的长度
    /// </summary>
    Num Length { get; }
    #endregion
    #region 向量的方向
    /// <summary>
    /// 获取向量的方向
    /// </summary>
    IUnit<IUTAngle> Direction { get; }
    #endregion
    #region 将极坐标转换为直角坐标
    /// <summary>
    /// 传入一个原点，返回极坐标向量的直角坐标
    /// </summary>
    /// <param name="originalPoint">用来作为原点的直角坐标，如果为<see langword="null"/>，默认为(0,0)</param>
    /// <returns></returns>
    IPoint ToPoint(IPoint? originalPoint = null)
    {
        var (len, direction) = this;
        var dir = direction.Convert(IUTAngle.Radian);
        var (r, t) = originalPoint ?? IPoint.Original;
        r += Math.Cos(dir) * len;
        t += Math.Sin(dir) * len;
        return new Point(r, t);
    }
    #endregion
    #region 解构向量
    /// <summary>
    /// 将本对象解构为长度和方向
    /// </summary>
    /// <param name="Vector">待解构的向量</param>
    /// <param name="length">向量的长度</param>
    /// <param name="direction">向量的方向</param>
    void Deconstruct(out Num length, out IUnit<IUTAngle> direction)
    {
        length = Length;
        direction = Direction;
    }
    #endregion
}
