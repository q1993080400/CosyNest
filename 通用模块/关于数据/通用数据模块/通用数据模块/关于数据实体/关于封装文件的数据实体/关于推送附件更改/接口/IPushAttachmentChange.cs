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
    /// <param name="changeInfo">描述对附件的更改的对象</param>
    /// <param name="services">一个用于请求服务的对象</param>
    /// <returns></returns>
    static abstract Task PushAttachmentChange(PushAttachmentChangeInfo changeInfo, IServiceProvider services);
    #endregion
}
