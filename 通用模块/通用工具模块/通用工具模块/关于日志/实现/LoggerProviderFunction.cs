using Microsoft.Extensions.Logging;

namespace System.Logging;

/// <summary>
/// 本类型可以提供一个单一的<see cref="ILogger"/>，
/// 它使用一个函数来记录日志
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="setLog">这个委托被用来写入日志，
/// 它的第一个参数是服务提供对象，第二个参数是发生的异常，
/// 第三个参数是记录日志时传入的状态对象</param>
/// <param name="serviceProvider">服务提供者对象，它可以用于请求其他服务</param>
sealed class LoggerProviderFunction(Func<IServiceProvider, Exception, object?, Task> setLog, IServiceProvider serviceProvider) : ILoggerProvider
{
    #region 公开成员
    #region 创建日志记录器
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
    #region 日志记录对象
    /// <summary>
    /// 获取唯一的一个日志记录对象
    /// </summary>
    private ILogger Logger { get; } = new LoggerFunction(setLog, serviceProvider);
    #endregion
    #endregion
}
