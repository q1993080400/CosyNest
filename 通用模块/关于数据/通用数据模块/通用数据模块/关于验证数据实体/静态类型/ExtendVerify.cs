using System.DataFrancis;

using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendData
{
    //这个部分类专门用来声明有关数据验证的扩展方法

    #region 添加数据验证委托
    /// <summary>
    /// 以单例模式注入一个<see cref="DataVerify"/>，
    /// 它可以对数据进行验证
    /// </summary>
    /// <param name="services">待添加的服务容器</param>
    /// <param name="dataVerifyInfo">用来创建<see cref="DataVerify"/>的参数</param>
    /// <returns></returns>
    public static IServiceCollection AddDataVerify(this IServiceCollection services, DataVerifyInfo? dataVerifyInfo = null)
        => services.AddSingleton(_ => CreateDataObj.DataVerifyDefault(dataVerifyInfo ?? new()));
    #endregion
}
