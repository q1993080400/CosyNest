using Microsoft.Extensions.Logging;

namespace System.Logging;

/// <summary>
/// 一个日志记录器创建者，
/// 它可以使用结构化的形式记录日志
/// </summary>
/// <typeparam name="LogObject">结构化日志的类型</typeparam>
/// <param name="createLog">用来创建日志的委托，
/// 它的第一个项是异常，第二个项是状态对象，返回值是结构化的日志</param>
/// <param name="setLog">用来写入日志的委托，
/// 它的第一个参数是结构化的日志，第二个参数是服务提供者对象</param>
/// <param name="serviceProvider">一个用于请求服务的对象</param>
sealed class LoggerProviderStructural<LogObject>
    (Func<Exception?, object?, LogObject> createLog,
    Func<LogObject, IServiceProvider, Task> setLog,
    IServiceProvider serviceProvider) : ILoggerProvider
{
    #region 公开成员
    #region 创建日志记录对象
    public ILogger CreateLogger(string categoryName)
        => Logger;
    #endregion
    #region 释放对象
    public void Dispose()
    {
    }
    #endregion
    #endregion
    #region 内部成员
    #region 日志记录器
    /// <summary>
    /// 返回唯一的一个日志记录器
    /// </summary>
    private ILogger Logger { get; }
        = new LoggerStructural<LogObject>(createLog, setLog, serviceProvider);
    #endregion
    #endregion
}
