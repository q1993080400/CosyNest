using System.DataFrancis;

using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门声明有关数据操作的扩展方法

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
}
