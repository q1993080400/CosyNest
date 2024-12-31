namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来推送对附件的更改
/// </summary>
public interface IPushAttachmentChange
{
    #region 推送附件更改
    /// <summary>
    /// 对比新附件和旧附件，
    /// 然后将附件的更改推送到存储附件的地方
    /// </summary>
    /// <param name="oldFiles">被替换的旧附件，在它当中存在，
    /// 但是在<paramref name="replaceFiles"/>中不存在的附件会被从存储层中删除</param>
    /// <param name="replaceFiles">要替换的新附件，在它当中存在，
    /// 但是在<paramref name="oldFiles"/>中不存在的附件会被保存到存储层</param>
    /// <param name="services">一个用于请求服务的对象</param>
    /// <returns></returns>
    static abstract Task PushAttachmentChange(IEnumerable<IHasPreviewFile> oldFiles, IEnumerable<IHasPreviewFile> replaceFiles, IServiceProvider services);
    #endregion
}
