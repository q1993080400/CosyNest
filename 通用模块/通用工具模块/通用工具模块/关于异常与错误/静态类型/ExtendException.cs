using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace System;

/// <summary>
/// 所有有关异常的扩展方法全部放在这里
/// </summary>
public static class ExtendException
{
    #region 记录异常
    /// <summary>
    /// 如果注入了<see cref="ILoggerProvider"/>，
    /// 则记录一个异常，否则不执行任何操作
    /// </summary>
    /// <param name="exception">要记录的异常</param>
    /// <param name="serviceProvider">服务请求者对象</param>
    /// <param name="additionalMessage">额外附加的消息</param>
    public static void Log(this Exception exception, IServiceProvider serviceProvider, string additionalMessage = "")
    {
        var log = serviceProvider.GetService<ILoggerProvider>();
        log?.CreateLogger("").Log(LogLevel.Error, default, additionalMessage, exception, static (_, _) => "");
    }
    #endregion
    #region 将不认识的枚举转换为异常
    /// <summary>
    /// 指示这个枚举无法识别，
    /// 并返回一个异常，稍后可以抛出它
    /// </summary>
    /// <typeparam name="Enum">枚举的类型</typeparam>
    /// <param name="enum">被标记为无法识别的枚举</param>
    /// <returns></returns>
    public static Exception Unrecognized<Enum>(this Enum @enum)
        where Enum : struct, System.Enum
        => new NotSupportedException($"无法识别枚举{@enum}");
    #endregion
}
