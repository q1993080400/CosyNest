namespace System;

public static partial class ExtendRazor
{
    //这个部分类专门声明有关上传的扩展方法

    #region 注入BrowserFileConvert
    #region 客户端版本
    /// <summary>
    /// 以单例模式注入一个适用于客户端渲染的<see cref="BrowserFileConvert"/>，
    /// 它可以将<see cref="IBrowserFile"/>转换为<see cref="IUploadFile"/>，
    /// 本服务依赖于服务<see cref="UploadFileOptions"/>
    /// </summary>
    /// <param name="services">待添加的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddBrowserFileConvertClient(this IServiceCollection services)
        => services.AddSingleton<BrowserFileConvert>(_ => (file, options) => new UploadFileClient(file, options.MaxAllowedSize));
    #endregion
    #region 服务端版本
    /// <summary>
    /// 以单例模式注入一个适用于服务端渲染的<see cref="BrowserFileConvert"/>，
    /// 它可以将<see cref="IBrowserFile"/>转换为<see cref="IUploadFile"/>，
    /// 本服务依赖于服务<see cref="UploadFileOptions"/>
    /// </summary>
    /// <param name="services">待添加的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddBrowserFileConvertServer(this IServiceCollection services)
        => services.AddSingleton<BrowserFileConvert>(_ => (file, options) => new UploadFileServer(file, options.MaxAllowedSize));
    #endregion
    #endregion
}
