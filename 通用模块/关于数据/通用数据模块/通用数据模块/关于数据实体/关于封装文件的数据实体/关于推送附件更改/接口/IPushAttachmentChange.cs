namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来推送对附件的更改
/// </summary>
/// <typeparam name="Obj">附件所依附的对象类型，
/// 通过它可以在执行操作后更改对象的状态</typeparam>
public interface IPushAttachmentChange<Obj>
    where Obj : class, IPushAttachmentChange<Obj>
{
    #region 推送附件更改
    /// <summary>
    /// 对比新附件和旧附件，
    /// 然后将附件的更改推送到存储附件的地方
    /// </summary>
    /// <param name="changeInfo">描述对附件的更改的对象</param>
    /// <param name="basePath">指示用来存储文件的基路径，
    /// 如果为<see langword="null"/>，则通过请求<see cref="RequestSaveBasePath"/>服务获取</param>
    /// <param name="services">一个用于请求服务的对象</param>
    /// <returns></returns>
    static abstract Task PushAttachmentChange(PushAttachmentChangeInfo<Obj> changeInfo, string? basePath, IServiceProvider services);
    #endregion
}
