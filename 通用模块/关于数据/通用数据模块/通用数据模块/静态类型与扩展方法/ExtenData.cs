using System.DataFrancis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System;

/// <summary>
/// 有关数据的扩展方法
/// </summary>
public static class ExtenData
{
    #region 关于依赖注入
    #region 注入一个简易的日志记录方法
    /// <summary>
    /// 注入一个简易的日志记录方法
    /// </summary>
    /// <typeparam name="Log">日志记录的类型</typeparam>
    /// <param name="loggingBuilder">日志记录器</param>
    /// <returns></returns>
    /// <inheritdoc cref="ExtenLogger.AddLoggerFunction{LogObject}(ILoggingBuilder, Func{Exception?, object?, LogObject}, Func{LogObject, IServiceProvider, Task}, bool)"/>
    public static ILoggingBuilder AddBusinessLoggingSimple<Log>(this ILoggingBuilder loggingBuilder,
        Func<Exception?, object?, Log> createLog)
        where Log : class
    {
        loggingBuilder.AddLoggerFunction(createLog, async (log, serviceProvider) =>
        {
            using var pipe = serviceProvider.RequiredDataPipe();
            await pipe.AddOrUpdate([log]);
        });
        loggingBuilder.SetMinimumLevel(LogLevel.Warning);
        return loggingBuilder;
    }
    #endregion
    #endregion
    #region 关于请求服务
    #region 请求IDataPipe
    /// <summary>
    /// 向服务容器请求一个<see cref="IDataContextFactory{Context}"/>，
    /// 并通过它创建一个<see cref="IDataPipe"/>返回
    /// </summary>
    /// <param name="serviceProvider">要请求的服务容器</param>
    /// <returns></returns>
    public static IDataPipe RequiredDataPipe(this IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<IDataContextFactory<IDataPipe>>().CreateContext();
    #endregion
    #endregion
}
