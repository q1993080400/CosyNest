using System.Numerics;

namespace System.MathFrancis;

public static partial class CreateBaseMath
{
    //这个部分类专门用来声明创建有关自增的对象的方法

    #region 创建自增时间
    /// <summary>
    /// 创建一个每次调用都会自增的时间
    /// </summary>
    /// <param name="dateTimeOffset">初始时间，
    /// 如果为<see langword="null"/>，默认为当前时间</param>
    /// <returns></returns>
    public static IIncremental<DateTimeOffset> IncrementalDate(DateTimeOffset? dateTimeOffset = null)
        => new IncrementalDateTimeOffset(dateTimeOffset ?? DateTimeOffset.Now);
    #endregion
    #region 创建递增数字
    /// <summary>
    /// 创建一个每次调用都会自增的数字
    /// </summary>
    /// <typeparam name="Obj">要自增的对象类型</typeparam>
    /// <param name="obj">递增的初始数据</param>
    /// <returns></returns>
    public static IIncremental<Obj> IncrementalNum<Obj>(Obj obj = default)
        where Obj : struct, IIncrementOperators<Obj>
        => new IncrementalNum<Obj>(obj);
    #endregion
}
