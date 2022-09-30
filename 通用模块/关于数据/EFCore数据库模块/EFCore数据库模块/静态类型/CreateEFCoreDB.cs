using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.DataFrancis.DB;
using System.DataFrancis.DB.EF;
using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个静态类可以用来创建和EFCore实现的数据管道有关的对象
/// </summary>
public static class CreateEFCoreDB
{
    #region 有关数据管道
    #region 获取本地连接字符串
    /// <summary>
    /// 获取本地连接字符串，
    /// 它使用Windwos身份验证连接到本机的SQLServer服务器
    /// </summary>
    /// <param name="dbName">数据库名称</param>
    /// <returns></returns>
    public static string ConnectionStringLocal(string dbName)
        => new SqlConnectionStringBuilder()
        {
            DataSource = "(local)",
            InitialCatalog = dbName,
            IntegratedSecurity = true
        }.ToString();
    #endregion
    #region 创建数据管道
    #region 指定工厂
    /// <summary>
    /// 创建一个底层使用EFCore实现的数据管道，
    /// 它支持表连接查询，但是不支持更新
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="EFDataPipe(Func{Type,DbContext})"/>
    public static IDataPipe Pipe(Func<Type, DbContext> createDbContext)
        => new EFDataPipe(createDbContext);
    #endregion
    #region 指定类型，直接使用无参数构造函数
    /// <typeparam name="DB">数据库上下文的类型</typeparam>
    /// <inheritdoc cref="Pipe(Func{Type,DbContext})"/>
    public static IDataPipe Pipe<DB>()
        where DB : DbContext, new()
        => Pipe(_ => new DB());
    #endregion
    #endregion
    #region 创建DbContext工厂
    #region 传入DbContext类型
    /// <summary>
    /// 创建一个<see cref="DbContext"/>工厂，
    /// 它能够根据传入的实体类的类型，
    /// 决定应该创建哪一个<see cref="DbContext"/>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="DbContextFactoryMerge(IEnumerable{Type})"/>
    public static Func<Type, DbContext> DbContextFactory(IEnumerable<Type> dbContextType)
        => new DbContextFactoryMerge(dbContextType).Create;
    #endregion
    #region 传入程序集，注册其中的所有DbContext
    /// <param name="dbContextAssembly">这个程序集中的所有公开，可创建的，
    /// 继承自<see cref="DbContext"/>的类型都会被注册进工厂中</param>
    /// <inheritdoc cref="DbContextFactory(IEnumerable{Type})"/>
    public static Func<Type, DbContext> DbContextFactory(Assembly dbContextAssembly)
    {
        var types = dbContextAssembly.GetTypes().
            Where(x => x.IsPublic && typeof(DbContext).IsAssignableFrom(x) && x.CanNew()).ToArray();
        return DbContextFactory(types);
    }
    #endregion
    #endregion
    #endregion
}
