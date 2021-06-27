using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Data;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 这个类型是<see cref="IDB"/>的实现，
    /// 可以视为一个数据库
    /// </summary>
    class DB : IDB
    {
        #region 获取数据库连接
        /// <summary>
        /// 调用这个委托以获取一个新的数据库连接，
        /// 要求连接字符串已经填好，而且没有副作用
        /// </summary>
        private Func<IDbConnection> Connection { get; }
        #endregion
        #region 接口实现
        #region 获取主键
        #region 缓存主键的字典
        /// <summary>
        /// 这个字典缓存数据表或视图的主键
        /// </summary>
        private IDictionary<string, string> PrimaryKey { get; }
        #endregion
        #region 正式方法
        public string? GetPrimaryKey(string Name)
            => PrimaryKey.TryGetValue(Name).Value;
        #endregion 
        #endregion
        #region 执行SQL脚本
        public async Task<IData[]> PerformSQL(string Script)
        {
            using var Connection = this.Connection();
            await Task.Run(() => Connection.Open());
            using var Transaction = await Task.Run(() => Connection.BeginTransaction());
            using var Command = Connection.CreateCommand();
            Command.CommandText = Script;
            Command.Transaction = Transaction;
            try
            {
                var datas = (await Task.Run(() => Command.ExecuteReader())).
                     Release(x => x.ToDatas().ToArray());
                Transaction.Commit();
                return datas;
            }
            catch (Exception)
            {
                await Task.Run(() => Transaction.Rollback());
                throw;
            }
        }
        #endregion
        #region 获取表或视图
        public IDataPipe GetTableOrView(string Name)
             => new TableOrView(this, Name);
        #endregion
        #region 用于生成SQL脚本的对象
        public ISQLGenerated SQLGenerated { get; }
        #endregion
        #endregion 
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Connection">调用这个委托以获取一个新的数据库连接，
        /// 要求连接字符串已经填好，而且没有副作用</param>
        /// <param name="PrimaryKey">这个元组集合枚举数据表的名称和主键的列名</param>
        /// <param name="SQLGenerated">用于生成SQL脚本的对象</param>
        public DB(Func<IDbConnection> Connection, IEnumerable<(string Table, string PrimaryKey)> PrimaryKey, ISQLGenerated SQLGenerated)
        {
            this.Connection = Connection;
            this.PrimaryKey = PrimaryKey.ToDictionary(true);
            this.SQLGenerated = SQLGenerated;
        }
        #endregion
    }
}
