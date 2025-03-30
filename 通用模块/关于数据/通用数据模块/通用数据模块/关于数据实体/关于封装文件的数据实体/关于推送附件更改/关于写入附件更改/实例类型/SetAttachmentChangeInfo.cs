using System.Linq.Expressions;
using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录可以作为写入附件更改的参数
/// </summary>
/// <typeparam name="Obj">实体的类型</typeparam>
public sealed class SetAttachmentChangeInfo<Obj>
    where Obj : class
{
    #region 数据库实体的类型
    /// <summary>
    /// 获取要写入的数据库实体
    /// </summary>
    public required Obj Entity { get; init; }
    #endregion
    #region 保存更改的基路径
    /// <summary>
    /// 如果发生更改，
    /// 这个属性会被用来作为<see cref="PushAttachmentChangeInfo{Obj}.SideEffect(string?)"/>的参数
    /// </summary>
    public required string? BasePath { get; init; }
    #endregion
    #region 获取副作用
    /// <summary>
    /// 获取实体附件更改的副作用
    /// </summary>
    public required Func<IServiceProvider, Task>? SideEffect { get; init; }
    #endregion
    #region 获取附件更改
    #region 传入IHasPreviewFile的集合
    /// <summary>
    /// 对数据库实体写入附件更改，
    /// 并返回一个对象，它封装了附件更改的副作用，
    /// 并可以用于进一步更改
    /// </summary>
    /// <typeparam name="Attachment">数据库实体包含的附件的类型</typeparam>
    /// <param name="accessFiles">一个用于访问数据库实体包含附件的表达式</param>
    /// <param name="replaceFiles">要替换数据库实体附件的新附件</param>
    /// <returns></returns>
    public SetAttachmentChangeInfo<Obj> SetChangeInfo<Attachment>(Expression<Func<Obj, IEnumerable<Attachment>>> accessFiles, IEnumerable<IHasPreviewFile?> replaceFiles)
        where Attachment : class, IWithID, IProjection<IFileObjectPosition>, ICreate<Attachment>, IPushAttachmentChange<Attachment>
    {
        if (accessFiles is not
            {
                Body: MemberExpression
                {
                    Member: PropertyInfo property
                }
            })
            throw new NotSupportedException("只接受一个直接访问属性的表达式");
        var attachments = property.GetValue<IEnumerable<Attachment>>(Entity);
        var changeInfo = attachments.GetChangeInfo(replaceFiles);
        if (!changeInfo.Changed)
            return new()
            {
                Entity = Entity,
                SideEffect = SideEffect.SideEffect(null),
                BasePath = BasePath
            };
        var newValue = property.PropertyType.CreateCollection(changeInfo.AfterChange);
        property.SetValue(Entity, newValue);
        return new()
        {
            Entity = Entity,
            SideEffect = SideEffect.SideEffect(changeInfo.SideEffect(BasePath)),
            BasePath = BasePath
        };
    }
    #endregion
    #region 传入可转换为IHasPreviewFile的对象
    /// <inheritdoc cref="SetChangeInfo{Attachment}(Expression{Func{Obj, IEnumerable{Attachment}}}, IEnumerable{IHasPreviewFile?})"/>
    public SetAttachmentChangeInfo<Obj> SetChangeInfo<Attachment>(Expression<Func<Obj, IEnumerable<Attachment>>> accessFiles,
        IEnumerable<IProjection<IHasPreviewFile?>> replaceFiles)
        where Attachment : class, IWithID, IProjection<IFileObjectPosition>, ICreate<Attachment>, IPushAttachmentChange<Attachment>
        => SetChangeInfo(accessFiles, replaceFiles.Projection().ToArray());
    #endregion
    #endregion 
}
