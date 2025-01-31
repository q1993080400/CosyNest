namespace System;

public static partial class ExtendBlazorWebAssembly
{
    //这个部分类专门声明有关上传的扩展方法

    #region 注入客户端上传服务
    /// <summary>
    /// 以单例模式注入适用于WebAssembly模式的上传服务
    /// </summary>
    /// <param name="services">待添加的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddFileUploadWebAssembly(this IServiceCollection services)
    {
        services.AddSingleton<BrowserFileConvert>(_ => (file, options) => new UploadFileClient(file, options.MaxAllowedSize));
        services.AddSingleton<UploadFileClientFactory>(_ => (coverUri, uri, uploadFile, id) => CreateDataObj.UploadFileClient(coverUri, uri, uploadFile, id));
        return services;
    }
    #endregion
}
