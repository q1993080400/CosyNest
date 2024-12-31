using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明和上传有关的扩展方法

    #region 添加对上传模型的绑定
    /// <summary>
    /// 添加一个上传模型绑定器，
    /// 它可以使控制器正确地识别<see cref="IHasPreviewFile"/>，以及它的集合
    /// </summary>
    /// <param name="list">上传模型绑定容器</param>
    /// <returns></returns>
    public static IList<IModelBinderProvider> AddPreviewFileModelBinder(this IList<IModelBinderProvider> list)
    {
        list.Insert(0, new HasPreviewFileModelBinderProvider());
        return list;
    }
    #endregion
}
