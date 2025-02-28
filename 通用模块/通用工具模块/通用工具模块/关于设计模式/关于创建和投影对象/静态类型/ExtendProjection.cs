using System.Design;

namespace System;

public static partial class ExtendDesign
{
    //这个部分类专门用来声明有关投影的扩展方法

    #region 投影对象
    /// <summary>
    /// 将一个集合的对象投影为另一种对象
    /// </summary>
    /// <param name="projections">要投影的对象集合</param>
    /// <returns></returns>
    /// <inheritdoc cref="IProjection{Obj}"/>
    public static IEnumerable<Obj> Projection<Obj>(this IEnumerable<IProjection<Obj>> projections)
        => projections.Select(x => x.Projection());
    #endregion
}
