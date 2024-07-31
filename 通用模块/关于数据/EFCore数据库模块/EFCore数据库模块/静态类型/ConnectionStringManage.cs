using Microsoft.Extensions.Configuration;

namespace System.DataFrancis.DB;

/// <summary>
/// 这个静态类可以用来管理连接字符串
/// </summary>
public static class ConnectionStringManage
{
    #region 设置连接字符串
    #region 直接设置
    /// <summary>
    /// 设置连接字符串
    /// </summary>
    /// <param name="connectionString">要设置的连接字符串的值</param>
    public static void SetConnectionString(string connectionString)
    {
        ConnectionStringField ??= connectionString;
    }
    #endregion
    #region 从配置中读取
    /// <summary>
    /// 从配置文件的Connection键中读取连接字符串，
    /// 并设置它
    /// </summary>
    /// <param name="configuration">用来读取连接字符串的配置文件</param>
    public static void SetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration["Connection"] ??
            throw new KeyNotFoundException("配置文件中不存在Connection键，无法获取数据库连接字符串");
        SetConnectionString(connectionString);
    }
    #endregion
    #endregion
    #region 获取连接字符串
    private static string? ConnectionStringField { get; set; }

    /// <summary>
    /// 获取数据库连接字符串
    /// </summary>
    public static string ConnectionString
        => ConnectionStringField ?? throw new NotSupportedException("尚未设置连接字符串");
    #endregion
}
