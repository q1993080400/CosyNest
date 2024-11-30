using System.DataFrancis;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明和上传有关的扩展方法

    #region 筛选启用的可上传对象
    /// <summary>
    /// 筛选集合中所有<see cref="ICanCancelPreviewFile.IsEnable"/>为<see langword="true"/>的<see cref="ICanCancelPreviewFile"/>
    /// </summary>
    /// <typeparam name="Obj">要筛选的集合的元素的类型</typeparam>
    /// <param name="objs">要筛选的集合</param>
    /// <returns></returns>
    public static IEnumerable<Obj> WhereEnable<Obj>(this IEnumerable<Obj?> objs)
        where Obj : ICanCancelPreviewFile
        => objs.Where(x => x is { IsEnable: true })!;
    #endregion
}
