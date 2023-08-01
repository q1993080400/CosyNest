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
    /// 注入一个简易的日志记录方法，
    /// 它只能使用<typeparamref name="Log"/>来作为记录类型
    /// </summary>
    /// <typeparam name="Log">日志记录的类型</typeparam>
    /// <param name="loggingBuilder">日志记录器</param>
    /// <returns></returns>
    public static ILoggingBuilder AddBusinessLoggingSimple<Log>(this ILoggingBuilder loggingBuilder)
        where Log : ExceptionRecord, new()
    {
        loggingBuilder.AddLoggerFunction(async (server, exception, _) =>
        {
            var ex = exception is { InnerException: { } e } ? e : exception;
            var method = ex.TargetSite;
            var log = new Log()
            {
                Date = DateTimeOffset.Now,
                Message = ex.Message,
                Stack = ex.StackTrace ?? "",
                Method = method is null ? "" : $"{method.DeclaringType}.{method.Name}",
                Exception = ex.GetType().Name,
            };
            var pipe = server.GetRequiredService<IDataPipe>();
            await pipe.AddOrUpdate(log);
        });
        loggingBuilder.SetMinimumLevel(LogLevel.Warning);
        return loggingBuilder;
    }
    #endregion
    #endregion
}
