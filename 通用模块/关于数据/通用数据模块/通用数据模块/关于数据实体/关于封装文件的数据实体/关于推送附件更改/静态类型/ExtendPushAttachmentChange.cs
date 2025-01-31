using System.DataFrancis;
using System.Design;

namespace System;

public static partial class ExtendData
{
    //这个部分类被用来声明有关推送附件更改有关的API

    #region 获取对附件的更改
    #region 传入IHasPreviewFile
    /// <summary>
    /// 获取对附件所做的更改
    /// </summary>
    /// <param name="oldFiles">被替换的旧附件，在它当中存在，
    /// 但是在<paramref name="replaceFiles"/>中不存在的附件会被判定为应该删除</param>
    /// <param name="replaceFiles">要替换的新附件，在它当中存在，
    /// 但是在<paramref name="oldFiles"/>中不存在的附件会被判定为应该保存</param>
    /// <returns></returns>
    public static PushAttachmentChangeInfo GetChangeInfo(this IEnumerable<IHasPreviewFile> oldFiles, IEnumerable<IHasPreviewFile> replaceFiles)
    {
        replaceFiles.CheckCannotUpload();
        var delete = oldFiles.ExceptBy(replaceFiles.Select(x => x.ID), x => x.ID).ToArray();
        return new()
        {
            Delete = delete,
            Add = replaceFiles.OfType<IHasUploadFileServer>().ToArray()
        };
    }
    #endregion
    #region 传入IProjection
    /// <inheritdoc cref="GetChangeInfo(IEnumerable{IHasPreviewFile}, IEnumerable{IHasPreviewFile})"/>
    public static PushAttachmentChangeInfo GetChangeInfo(this IEnumerable<IProjection<IHasPreviewFile>> oldFiles, IEnumerable<IHasPreviewFile> replaceFiles)
        => oldFiles.Projection().ToArray().GetChangeInfo(replaceFiles);
    #endregion
    #endregion
}
