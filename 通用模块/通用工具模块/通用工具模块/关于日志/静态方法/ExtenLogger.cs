using System.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System;

/// <summary>
/// 有关日志的扩展方法全部放在这里
/// </summary>
public static class ExtenLogger
{
    #region 添加日志记录函数
    /// <summary>
    /// 添加一个日志记录函数
    /// </summary>
    /// <param name="builder">日志创建器对象</param>
    /// <param name="clearProviders">如果这个值为<see langword="true"/>，
    /// 则会移除掉现有的日志提供程序</param>
    /// <returns></returns>
    /// <inheritdoc cref="LoggerProviderFunction(Func{IServiceProvider, Exception, object?, Task}, IServiceProvider)"/>
    public static ILoggingBuilder AddLoggerFunction(this ILoggingBuilder builder, Func<IServiceProvider, Exception, object?, Task> setLog, bool clearProviders = false)
    {
        if (clearProviders)
            builder.ClearProviders();
        builder.Services.AddSingleton<ILoggerProvider>(x => new LoggerProviderFunction(setLog, x));
        return builder;
    }

    /*注意：这个服务只能以单例模式注入，
      这是因为ILoggerProviderFactory是一个单例服务，
      它只能请求单例服务*/
    #endregion
}
