﻿using System.NetFrancis;

using Microsoft.JSInterop;

namespace System;

public static partial class ExtendBlazorServer
{
    //这个部分类专门用来声明和Http有关的扩展方法

    #region 注入IHostProvide
    /// <summary>
    /// 以范围模式注入一个<see cref="IHostProvide"/>，
    /// 它专门用于Server模式，可以用来提供主机Uri，注意：
    /// 它同时注入了一个<see cref="TagLazy{Obj}"/>服务，
    /// 你必须先手动调用<see cref="TagLazy{Obj}.Initialization"/>方法，
    /// 才能够正常访问主机Uri
    /// </summary>
    /// <param name="services">要注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddHostProvideServer(this IServiceCollection services)
    {
        services.AddScoped(serviceProvider =>
        {
            var jsWindow = serviceProvider.GetRequiredService<IJSWindow>();
            return new TagLazy<string>(async () =>
            {
                var href = await jsWindow.Location.Href.Get();
                var uri = new UriComplete(href);
                return uri.UriHost.ThrowIfNull();
            });
        });
        services.AddScoped(serviceProvider =>
        {
            var tag = serviceProvider.GetRequiredService<TagLazy<string>>();
            return CreateNet.HostProvide(tag.CheckContent());
        });
        return services;
    }
    #endregion
}
