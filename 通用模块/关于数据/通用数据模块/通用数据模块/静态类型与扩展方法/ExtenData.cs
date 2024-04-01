using System.DataFrancis;
using System.DataFrancis.EntityDescribe;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System;

/// <summary>
/// 有关数据的扩展方法
/// </summary>
public static class ExtenData
{
    #region 为数据管道添加验证功能
    /// <summary>
    /// 为数据管道增加验证功能，
    /// 并返回支持验证的数据管道
    /// </summary>
    /// <param name="pipe">原数据管道</param>
    /// <param name="verify">用来验证的委托，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns></returns>
    public static IDataPipe UseVerify(this IDataPipe pipe, DataVerify? verify = null)
        => new DataPipeVerify(pipe, verify ??= CreateDataObj.DataVerifyDefault());
    #endregion
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
            var pipe = serviceProvider.GetRequiredService<IDataPipe>();
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
    /// 向服务容器请求一个<see cref="IDataPipe"/>
    /// </summary>
    /// <param name="serviceProvider">要请求的服务容器</param>
    /// <returns></returns>
    public static IDataPipe RequestDataPipe(this IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<IDataPipe>();
    #endregion
    #endregion
}
