using Microsoft.Data.SqlClient;

using NetTopologySuite;
using NetTopologySuite.Geometries;

using System.DataFrancis.DB;
using System.DataFrancis.DB.EF;
using System.Reflection;
using System.Text.Json.Serialization;
using System.TreeObject.Json;

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
    #region 返回用来转换Point的转换器
    /// <summary>
    /// 返回用来转换<see cref="Point"/>的Json转换器
    /// </summary>
    public static JsonConverter<Point> JsonPoint { get; }
        = CreateJson.JsonMap<Point, PointMap>(x => new PointMap()
        {
            X = x.X,
            Y = x.Y,
            Z = x.Z is double.NaN ? null : x.Z,
            SRID = x.SRID
        },
            x => x.SRID is SRID ?
            GeometryFactory.CreatePoint(new Coordinate(x.X, x.Y)) :
            new Point(x.X, x.Y, x.Z ?? double.NaN)
            {
                SRID = x.SRID
            });
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
    #region 创建数据管道
    #region 指定工厂
    /// <summary>
    /// 创建一个底层使用EFCore实现的数据管道
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="EFDataPipe(Func{Type,DbContextFrancis})"/>
    public static IDataPipeDB Pipe(Func<Type, DbContextFrancis> createDbContext)
        => new EFDataPipe(createDbContext);
    #endregion
    #region 指定类型，直接使用无参数构造函数
    /// <typeparam name="DB">数据库上下文的类型</typeparam>
    /// <inheritdoc cref="Pipe(Func{Type,DbContextFrancis})"/>
    public static IDataPipeDB Pipe<DB>()
        where DB : DbContextFrancis, new()
        => Pipe(_ => new DB());
    #endregion
    #endregion
    #region 创建DbContext工厂
    #region 传入DbContext类型
    /// <summary>
    /// 创建一个<see cref="DbContextFrancis"/>工厂，
    /// 它能够根据传入的实体类的类型，
    /// 决定应该创建哪一个<see cref="DbContextFrancis"/>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="DbContextFactoryMerge(IEnumerable{Type})"/>
    public static Func<Type, DbContextFrancis> DbContextFactory(IEnumerable<Type> dbContextType)
        => new DbContextFactoryMerge(dbContextType).Create;
    #endregion
    #region 传入程序集，注册其中的所有DbContext
    /// <param name="dbContextAssembly">这个程序集中的所有公开，可创建的，
    /// 继承自<see cref="DbContextFrancis"/>的类型都会被注册进工厂中</param>
    /// <inheritdoc cref="DbContextFactory(IEnumerable{Type})"/>
    public static Func<Type, DbContextFrancis> DbContextFactory(Assembly dbContextAssembly)
    {
        var types = dbContextAssembly.GetTypes().
            Where(x => x.IsPublic && typeof(DbContextFrancis).IsAssignableFrom(x) && x.CanNew()).ToArray();
        return DbContextFactory(types);
    }
    #endregion
    #endregion
    #endregion
}

#region Json投影类型
file class PointMap
{
    public double X { get; set; }
    public double Y { get; set; }
    public double? Z { get; set; }
    public int SRID { get; set; }
}
#endregion
