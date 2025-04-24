using System.DataFrancis;

using Microsoft.Extensions.Logging;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关数据实体的扩展方法

    #region 添加日志记录方法
    /// <summary>
    /// 添加一个日志记录方法，
    /// 它通过<see cref="LogException"/>来记录日志
    /// </summary>
    /// <param name="loggingBuilder">用来创建日志记录器的对象</param>
    public static void AddBusinessLoggingSimple(this ILoggingBuilder loggingBuilder)
        => loggingBuilder.AddBusinessLoggingSimple(CreateDataObj.LogException<LogException>);
    #endregion
}
