using System.DataFrancis;
using System.Linq.Expressions;

namespace System;

public static partial class ExtendData
{
    //这个部分类被用来声明有关写入附件更改有关的API

    #region 写入附件更改
    #region 传入IHasPreviewFile的集合
    /// <summary>
    /// 对数据库实体写入附件更改，
    /// 并返回一个对象，它封装了附件更改的副作用，
    /// 并可以用于进一步更改
    /// </summary>
    /// <typeparam name="Obj">数据库实体的类型</typeparam>
    /// <typeparam name="Attachment">数据库实体包含的附件的类型</typeparam>
    /// <param name="obj">要写入附件更改的数据库实体</param>
    /// <param name="basePath">如果调用存在副作用，它将作为保存文件的基路径</param>
    /// <returns></returns>
    /// <inheritdoc cref="SetAttachmentChangeInfo{Obj}.SetChangeInfo{Attachment}(Expression{Func{Obj, IEnumerable{Attachment}}}, IEnumerable{IHasPreviewFile?})"/>
    public static SetAttachmentChangeInfo<Obj> SetChangeInfo<Obj, Attachment>(this Obj obj,
        Expression<Func<Obj, IEnumerable<Attachment>>> accessFiles,
        IEnumerable<IHasPreviewFile?> replaceFiles, string? basePath = null)
        where Obj : class, IEntity
        where Attachment : class, IWithID, IProjection<IFileObjectPosition>, ICreate<Attachment>, IPushAttachmentChange<Attachment>
    {
        var setChangeInfo = new SetAttachmentChangeInfo<Obj>()
        {
            BasePath = basePath,
            Entity = obj,
            SideEffect = null
        };
        return setChangeInfo.SetChangeInfo(accessFiles, replaceFiles);
    }
    #endregion
    #region 传入可转换为IHasPreviewFile的对象
    /// <inheritdoc cref="SetChangeInfo{Obj, Attachment}(Obj, Expression{Func{Obj, IEnumerable{Attachment}}}, IEnumerable{IHasPreviewFile?}, string?)"/>
    public static SetAttachmentChangeInfo<Obj> SetChangeInfo<Obj, Attachment>(this Obj obj,
        Expression<Func<Obj, IEnumerable<Attachment>>> accessFiles,
        IEnumerable<IProjection<IHasPreviewFile?>> replaceFiles, string? basePath = null)
        where Obj : class, IEntity
        where Attachment : class, IWithID, IProjection<IFileObjectPosition>, ICreate<Attachment>, IPushAttachmentChange<Attachment>
        => obj.SetChangeInfo(accessFiles, replaceFiles.Projection().ToArray(), basePath);
    #endregion
    #endregion
}
