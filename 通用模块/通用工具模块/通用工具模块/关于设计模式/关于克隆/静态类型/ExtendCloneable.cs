namespace System;

public static partial class ExtendDesign
{
    //这个部分类专门用来声明有关克隆的扩展方法

    #region 批量克隆
    /// <summary>
    /// 批量执行克隆
    /// </summary>
    /// <typeparam name="Obj">要克隆的目标类型</typeparam>
    /// <param name="objs">要克隆的集合</param>
    /// <returns></returns>
    public static List<Obj> Cloneable<Obj>(this IEnumerable<Obj> objs)
        where Obj : ICloneable<Obj>
        => [.. objs.Select(x => x.Cloneable())];
    #endregion
}
