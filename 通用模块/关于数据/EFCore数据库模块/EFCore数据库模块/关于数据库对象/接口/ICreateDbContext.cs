using Microsoft.EntityFrameworkCore;

namespace System.DataFrancis.DB;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以根据连接字符串来创建数据库上下文
/// </summary>
/// <typeparam name="DB">数据库上下文的类型</typeparam>
public interface ICreateDbContext<DB>
    where DB : DbContext, ICreateDbContext<DB>
{
    #region 创建数据上下文
    /// <summary>
    /// 根据连接字符串，创建一个数据上下文
    /// </summary>
    /// <param name="connectionString">连接字符串</param>
    /// <returns></returns>
    abstract static DB Create(string connectionString);
    #endregion
}
