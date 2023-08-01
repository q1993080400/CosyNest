using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System;

/// <summary>
/// 所有有关异常的扩展方法全部放在这里
/// </summary>
public static class ExtenException
{
    #region 记录异常
    /// <summary>
    /// 如果注入了<see cref="ILoggerProvider"/>，
    /// 则记录一个异常，否则不执行任何操作
    /// </summary>
    /// <param name="exception">要记录的异常</param>
    /// <param name="serviceProvider">服务请求者对象</param>
    public static void Log(this Exception exception, IServiceProvider serviceProvider)
    {
        var log = serviceProvider.GetService<ILoggerProvider>();
        log?.CreateLogger("").Log(LogLevel.Error, exception, "");
    }
    #endregion
}
