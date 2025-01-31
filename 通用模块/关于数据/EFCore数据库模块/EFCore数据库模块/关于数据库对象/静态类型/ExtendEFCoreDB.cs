using System.DataFrancis;
using System.DataFrancis.DB;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System;

public static partial class ExtendEFCoreDB
{
    //这个部分类专门用来声明和数据库对象有关的扩展方法

    #region 关于依赖注入
    #region 注入数据上下文工厂
    /// <summary>
    /// 从配置文件中读取连接字符串，
    /// 并以单例模式注入一个<see cref="IDataContextFactory{Context}"/>，
    /// 它同时还会以瞬时模式注入一个<typeparamref name="DB"/>服务，
    /// 这个是给EFCore设计时工具准备的
    /// </summary>
    /// <typeparam name="DB">数据上下文的类型</typeparam>
    /// <param name="services">要注入的依赖注入容器</param>
    /// <returns></returns>
    public static IServiceCollection AddDataContextFactory<DB>(this IServiceCollection services)
        where DB : DbContext, ICreateDbContext<DB>
    {
        #region 用来获取连接字符的本地函数
        static string GetConnectionString(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            const string key = "Connection";
            return configuration[key] ??
                throw new KeyNotFoundException($"配置文件中不存在{key}键，无法获取数据库连接字符串");
        }
        #endregion
        services.AddSingleton(serviceProvider =>
        {
            var connectionString = GetConnectionString(serviceProvider);
            return CreateEFCoreDB.DataContextFactory(() => DB.Create(connectionString));
        });
        return services.AddTransient(serviceProvider =>
        {
            var connectionString = GetConnectionString(serviceProvider);
            return DB.Create(connectionString);
        });
    }
    #endregion
    #endregion
}
