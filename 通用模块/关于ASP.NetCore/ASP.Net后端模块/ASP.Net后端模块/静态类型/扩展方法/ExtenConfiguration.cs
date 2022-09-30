using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

using System.Reflection;
using System.Text.Json.Serialization;
using System.TreeObject.Json;

using static Microsoft.AspNetCore.CreateWebApi;

namespace System;

public static partial class ExtenWebApi
{
    //这个部分类专门用来声明有关配置的扩展方法

    #region 配置MvcOptions
    #region 添加常用类型Json的支持
    /// <summary>
    /// 为<see cref="MvcOptions"/>添加常用类型Json的支持
    /// </summary>
    /// <param name="options">待添加支持的Mvc配置</param>
    public static void AddFormatterJson(this MvcOptions options)
    {
        #region 添加格式化器的本地函数
        void Add(params JsonConverter[] items)
        {
            foreach (var item in items.Select(x => x.Fit()))
            {
                options.InputFormatters.Insert(0, InputFormatterJson(item));
                options.OutputFormatters.Insert(0, OutputFormatterJson(item));
            }
        }
        #endregion
        Add(CreateJson.SerializationCommon.ToArray());
    }
    #endregion
    #endregion
    #region 配置SignalR
    #region 注册所有SignalR中心
    /// <summary>
    /// 注册一个程序集中的所有SignalR中心
    /// </summary>
    /// <param name="builder">执行注册的路由生成器协定</param>
    /// <param name="assembly">SignalR中心所在的程序集，
    /// 该程序集中所有继承自<see cref="Hub"/>的公开类型都会被注册</param>
    /// <param name="pattern">用来获取路由模板的委托，
    /// 它的参数是继承自<see cref="Hub"/>的类型名称，返回值是路由模板，
    /// 如果为<see langword="null"/>，则默认路由模式为/Hub/{TypeName}，
    /// 如果存在，它还会删除类型名称末尾的Hub后缀</param>
    public static void MapHubAll(this IEndpointRouteBuilder builder, Assembly assembly, Func<string, string>? pattern = null)
    {
        pattern ??= x => $"/Hub/{x.Trim(false, "Hub")}";
        var method = typeof(HubEndpointRouteBuilderExtensions).GetTypeData().
            FindMethod(nameof(HubEndpointRouteBuilderExtensions.MapHub), CreateReflection.MethodSignature(typeof(HubEndpointConventionBuilder), typeof(IEndpointRouteBuilder), typeof(string)));
        foreach (var item in assembly.GetTypes().Where(x => typeof(Hub).IsAssignableFrom(x) && x.IsPublic && !x.IsAbstract))
        {
            method.MakeGenericMethod(item).Invoke<object>(null, builder, pattern(item.Name));
        }
    }
    #endregion
    #endregion
    #region 配置SingleServiceProvider
    /// <summary>
    /// 配置<see cref="CreateASP.SingleServiceProvider"/>，
    /// 使它能够被使用
    /// </summary>
    /// <param name="host">函数通过<see cref="IHost.Services"/>来获取<see cref="IServiceProvider"/></param>
    public static void SetSingleServiceProvider(this IHost host)
        => CreateASP.SingleServiceProvider = host.Services;

    /*本方法存在以下隐患：
      如果使用本方法进行初始化，
      不要在单例服务初始化时访问CreateASP.SingleServiceProvider，
      这是因为在WebApplicationBuilder.Build()调用后才会执行本方法，
      它发生在单例服务初始化之后*/
    #endregion
}
