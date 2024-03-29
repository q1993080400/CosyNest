﻿using Microsoft.Extensions.Logging;

namespace System.Logging;

/// <summary>
/// 这个类型使用一个函数来记录日志
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="setLog">这个委托被用来写入日志，
/// 它的第一个参数是服务提供对象，第二个参数是发生的异常，
/// 第三个参数是记录日志时传入的状态对象</param>
/// <param name="serviceProvider">服务提供者对象，它可以用于请求其他服务</param>
sealed class LoggerFunction(Func<IServiceProvider, Exception, object?, Task> setLog, IServiceProvider serviceProvider) : ILogger
{
    #region 公开成员
    #region 记录日志
    public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel) || exception is null or BusinessException)
            return;
        await SetLog(ServiceProvider, exception.InnerException ?? exception, state);
    }
    #endregion
    #region 是否启用
    public bool IsEnabled(LogLevel logLevel)
        => logLevel is not LogLevel.None;
    #endregion
    #region 开始逻辑操作范围
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => null;
    #endregion 
    #endregion
    #region 内部成员
    #region 用来写入日志的委托
    /// <summary>
    /// 这个委托被用来写入日志，
    /// 它的第一个参数是服务提供对象，
    /// 第二个参数是发生的异常，
    /// 第三个参数是记录日志时传入的状态对象
    /// </summary>
    private Func<IServiceProvider, Exception, object?, Task> SetLog { get; } = setLog;
    #endregion
    #region 服务提供者对象
    /// <summary>
    /// 获取服务提供者对象，它可以用于请求其他服务
    /// </summary>
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    #endregion
    #endregion
}
