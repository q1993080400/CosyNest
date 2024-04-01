using Microsoft.Extensions.Logging;

namespace System.Logging;

/// <summary>
/// 这个类型是<see cref="ILogger"/>的实现，
/// 它是一个结构化的日志记录器
/// </summary>
/// <inheritdoc cref="LoggerProviderStructural{LogObject}"/>
/// <inheritdoc cref="LoggerProviderStructural{LogObject}.LoggerProviderStructural(Func{Exception?, object?, LogObject}, Func{LogObject, IServiceProvider, Task}, IServiceProvider)"/>
sealed class LoggerStructural<LogObject>
    (Func<Exception?, object?, LogObject> createLog,
    Func<LogObject, IServiceProvider, Task> setLog,
    IServiceProvider serviceProvider) : ILogger
{
    #region 写入日志
    public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;
        var log = createLog(exception, state);
        await setLog(log, serviceProvider);
    }
    #endregion
    #region 是否启用
    public bool IsEnabled(LogLevel logLevel)
        => logLevel >= LogLevel.Error;
    #endregion
    #region 开始范围
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => null;
    #endregion 
}
