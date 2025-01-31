using System.DataFrancis;

using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门用来声明有关筛选数据实体的扩展方法方法

    #region 注入表达式解析器
    /// <summary>
    /// 以单例模式注入一个<see cref="IDataFilterAnalysis"/>，
    /// 它可以用来将搜索条件解析为对表达式的调用
    /// </summary>
    /// <param name="services">要注入的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddDataFilterAnalysis(this IServiceCollection services)
        => services.AddSingleton(_ => CreateDataObj.DataFilterAnalysis);
    #endregion
}
