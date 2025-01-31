using System.DataFrancis.DB.EF;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace System.DataFrancis;

/// <summary>
/// 这个静态类可以用来创建和EFCore实现的数据管道有关的对象
/// </summary>
public static class CreateEFCoreDB
{
    #region 有关空间数据
    #region SRID常量
    /// <summary>
    /// 获取空间标识符常量，
    /// 按照规范，项目中所有<see cref="Point"/>都使用这个空间标识符
    /// </summary>
    private const int SRID = 4326;
    #endregion
    #region 返回几何工厂
    /// <summary>
    /// 获取一个用来创建几何图形的工厂
    /// </summary>
    public static GeometryFactory GeometryFactory { get; } = NtsGeometryServices.Instance.CreateGeometryFactory(SRID);
    #endregion
    #endregion
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
            IntegratedSecurity = true,
            Encrypt = true,
            TrustServerCertificate = true,
        }.ToString();
    #endregion
    #region 创建数据上下文工厂
    /// <summary>
    /// 创建一个数据上下文工厂，并返回
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="EFDataContextFactory(Func{DbContext})"/>
    public static IDataContextFactory<IDataPipe> DataContextFactory(Func<DbContext> factory)
        => new EFDataContextFactory(factory);
    #endregion
    #endregion
}
