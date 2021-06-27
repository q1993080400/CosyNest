using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 这个静态类可以用来帮助创建和数据库有关的API
    /// </summary>
    public static class CreateDB
    {
        #region 创建IDB
        #region 指定连接方式
        /// <summary>
        /// 使用指定的连接方式创建一个<see cref="IDB"/>
        /// </summary>
        /// <param name="Connection">调用这个委托以获取一个新的数据库连接，
        /// 要求连接字符串已经填好，而且没有副作用</param>
        /// <param name="PrimaryKey">这个元组集合枚举数据表的名称和主键的列名，
        /// 如果为<see langword="null"/>，代表没有主键</param>
        /// <param name="SQLGenerated">用于生成SQL脚本的对象，
        /// 如果为<see langword="null"/>，则使用一个默认的对象</param>
        /// <returns>新创建的<see cref="IDB"/></returns>
        public static IDB DB(Func<IDbConnection> Connection, IEnumerable<(string Table, string PrimaryKey)>? PrimaryKey = null, ISQLGenerated? SQLGenerated = null)
            => new DB(Connection,
                PrimaryKey ?? new (string, string)[] { },
                SQLGenerated ?? DataFrancis.DB.SQLGenerated.Only);
        #endregion
        #region 指定连接器类型
        /// <summary>
        /// 使用指定的连接字符串初始化<see cref="IDB"/>
        /// </summary>
        /// <typeparam name="Connection">数据库连接器的类型</typeparam>
        /// <param name="ConnectionText">数据库连接字符串</param>
        /// <param name="PrimaryKey">这个元组集合枚举数据表的名称和主键的列名，
        /// 如果为<see langword="null"/>，代表没有主键</param>
        /// <param name="SQLGenerated">用于生成SQL脚本的对象，
        /// 如果为<see langword="null"/>，则使用一个默认的对象</param>
        /// <returns>新创建的<see cref="IDB"/></returns>
        public static IDB DB<Connection>(string ConnectionText, IEnumerable<(string Table, string PrimaryKey)>? PrimaryKey = null, ISQLGenerated? SQLGenerated = null)
            where Connection : IDbConnection, new()
            => DB(() => new Connection()
            {
                ConnectionString = ConnectionText
            }, PrimaryKey, SQLGenerated);
        #endregion
        #endregion
    }
}
