using System.DataFrancis;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
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
    #region 关于请求服务
    #region 请求IDataPipe
    /// <summary>
    /// 向服务容器请求一个<see cref="IDataContextFactory{Context}"/>，
    /// 并通过它创建一个<see cref="IDataPipe"/>返回
    /// </summary>
    /// <param name="serviceProvider">要请求的服务容器</param>
    /// <returns></returns>
    public static IDataPipe RequiredDataPipe(this IServiceProvider serviceProvider)
        => serviceProvider.GetRequiredService<IDataContextFactory<IDataPipe>>().CreateContext();
    #endregion
    #endregion
    #region 关于枚举
    #region 获取枚举的值和描述
    /// <summary>
    /// 获取一个迭代器，它枚举枚举的值以及描述
    /// </summary>
    /// <param name="type">枚举的类型，
    /// 如果它不是枚举，则返回一个空集合</param>
    /// <returns></returns>
    public static IEnumerable<(Enum Value, string Describe)> GetEnumDescription(this Type type)
    {
        if (!type.IsEnum)
            yield break;
        foreach (var field in type.GetFields().Where(static x => x.IsStatic))
        {
            var @enum = (Enum)field.GetValue(null)!;
            var renderDataName = field.GetCustomAttribute<RenderDataAttribute>()?.Name;
            yield return (@enum, renderDataName ?? @enum.ToString());
        }
    }
    #endregion
    #region 获取枚举的描述
    /// <summary>
    /// 获取枚举的描述，
    /// 如果没有描述，返回枚举的字面量
    /// </summary>
    /// <param name="enum">待返回描述的枚举</param>
    /// <returns></returns>
    public static string GetDescription(this Enum @enum)
    {
        var description = @enum.GetType().GetField(@enum.ToString())?.
            GetCustomAttribute<RenderDataAttribute>();
        return description?.Name ?? @enum.ToString();
    }
    #endregion
    #endregion 
}
