using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个对象是<see cref="IIncremental{Obj}"/>的实现，
/// 可以返回一个递增的对象
/// </summary>
/// <typeparam name="Obj">要递增的对象的类型</typeparam>
/// <param name="seed">要递增的初始值</param>
sealed class IncrementalNum<Obj>(Obj seed) : IIncremental<Obj>
    where Obj : struct, IIncrementOperators<Obj>
{
    #region 递增对象
    public Obj Incremental()
        => seed++;
    #endregion
}
