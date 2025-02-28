using System.DataFrancis;

namespace System;

public static partial class ExtendData
{
    //这个部分类被用来声明有关推送附件更改有关的API

    #region 获取对附件的更改
    #region 传入IHasPreviewFile的集合
    /// <summary>
    /// 获取对附件所做的更改
    /// </summary>
    /// <param name="oldFiles">被替换的旧附件，在它当中存在，
    /// 但是在<paramref name="replaceFiles"/>中不存在的附件会被判定为应该删除</param>
    /// <param name="replaceFiles">要替换的新附件，在它当中存在，
    /// 但是在<paramref name="oldFiles"/>中不存在的附件会被判定为应该保存</param>
    /// <returns></returns>
    /// <inheritdoc cref="IPushAttachmentChange{Obj}"/>
    public static PushAttachmentChangeInfo<Obj> GetChangeInfo<Obj>(this IEnumerable<Obj> oldFiles, IEnumerable<IHasPreviewFile?> replaceFiles)
        where Obj : class, IWithID, IProjection<IFileObjectPosition>, ICreate<Obj>, IPushAttachmentChange<Obj>
    {
        replaceFiles.CheckCannotUpload();
        var replaceFilesIds = replaceFiles.WhereEnable().Select(x => x.ID).ToHashSet();
        var (notChange, change) = oldFiles.Split(x => replaceFilesIds.Contains(x.ID));
        var delete = change.Select(x => new AttachmentDeleteInfo<Obj>()
        {
            Depend = x,
            FilePosition = x.Projection()
        }).ToArray();
        var oldFileDictionary = oldFiles.ToDictionary();
        var add = replaceFiles.OfType<IHasUploadFileServer>().
            Select(x => new AttachmentAddInfo<Obj>()
            {
                Depend = Obj.Create(),
                UploadFile = x
            }).ToArray();
        return new()
        {
            Delete = delete,
            Add = add,
            AfterChange = [.. notChange, .. add.Select(x => x.Depend)]
        };
    }
    #endregion 
    #region 传入可转换为IHasPreviewFile的对象
    /// <inheritdoc cref="GetChangeInfo{Obj}(IEnumerable{Obj}, IEnumerable{IHasPreviewFile?})"/>
    public static PushAttachmentChangeInfo<Obj> GetChangeInfo<Obj>(this IEnumerable<Obj> oldFiles, IEnumerable<IProjection<IHasPreviewFile?>> replaceFiles)
        where Obj : class, IWithID, IProjection<IFileObjectPosition>, ICreate<Obj>, IPushAttachmentChange<Obj>
        => oldFiles.GetChangeInfo(replaceFiles.Projection().ToArray());
    #endregion
    #endregion
}
