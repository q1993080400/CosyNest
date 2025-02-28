using Microsoft.Extensions.Logging;

namespace System;

/// <summary>
/// 有关数据的扩展方法
/// </summary>
public static partial class ExtendData
{
    #region 关于依赖注入
    #region 注入一个简易的日志记录方法
    /// <summary>
    /// 注入一个简易的日志记录方法
    /// </summary>
    /// <typeparam name="Log">日志记录的类型</typeparam>
    /// <param name="loggingBuilder">日志记录器</param>
    /// <returns></returns>
    /// <inheritdoc cref="ExtendLogger.AddLoggerFunction{LogObject}(ILoggingBuilder, Func{Exception?, object?, LogObject}, Func{LogObject, IServiceProvider, Task}, bool)"/>
    public static ILoggingBuilder AddBusinessLoggingSimple<Log>(this ILoggingBuilder loggingBuilder,
        Func<Exception?, object?, Log> createLog)
        where Log : class
    {
        loggingBuilder.AddLoggerFunction(createLog, async (log, serviceProvider) =>
        {
            await using var pipe = serviceProvider.RequiredDataPipe();
            await pipe.Push(context => context.AddOrUpdate([log]));
        });
        return loggingBuilder;
    }
    #endregion
    #endregion
}
