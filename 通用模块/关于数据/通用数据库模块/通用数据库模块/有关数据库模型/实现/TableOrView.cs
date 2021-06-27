using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 这个对象代表数据库中的一张数据表或视图
    /// </summary>
    class TableOrView : IDataPipe
    {
        #region 有关数据库
        #region 数据库对象
        /// <summary>
        /// 这个对象被用于与数据库进行通讯
        /// </summary>
        private DB DB { get; }
        #endregion
        #region 数据表的名称
        /// <summary>
        /// 获取数据表或视图的名称
        /// </summary>
        private string TableName { get; }
        #endregion
        #region 数据表的主键
        /// <summary>
        /// 获取数据表的主键，
        /// 如果为<see langword="null"/>，代表没有主键
        /// </summary>
        private string? PrimaryKeyName { get; }
        #endregion
        #endregion
        #region 关于查询和添加数据
        #region 辅助方法：将数据与数据库绑定
        /// <summary>
        /// 辅助方法，将数据与数据库绑定
        /// </summary>
        /// <param name="Data">待绑定的数据</param>
        /// <param name="Binding">如果这个值为<see langword="true"/>，则执行绑定，否则不进行绑定</param>
        private void Binding(IData Data, bool Binding)
        {
            if (Binding && PrimaryKeyName != null)
            {
                var PK = Data[PrimaryKeyName];
                if (PK == null)
                    throw new NullReferenceException($"主键列{PrimaryKeyName}的值为null");
                Data.Binding = new DBBinding(DB, TableName, PrimaryKeyName, PK);
            }
        }
        #endregion
        #region 获取是否支持绑定
        public bool CanBinding => true;
        #endregion
        #region 关于查询数据
        #region 同步方法
        public IEnumerable<IData> Query(Expression<Func<PlaceholderData, bool>>? Expression, bool Binding)
            => QueryAsyn(Expression, Binding).ToEnumerable();
        #endregion
        #region 异步方法
        public async IAsyncEnumerable<IData> QueryAsyn(Expression<Func<PlaceholderData, bool>>? Expression, bool Binding)
        {
            if (Expression == null)
                throw new ArgumentNullException("由于数据库的体量可能非常庞大，因此必须要有查询条件");
            foreach (var item in await DB.PerformSQL(DB.SQLGenerated.ScriptInquire(Expression, TableName)))
            {
                this.Binding(item, Binding);
                yield return item;
            }
        }
        #endregion
        #endregion
        #region 关于添加数据
        #region 同步方法
        public void Add(IEnumerable<IData> Data, bool Binding)
            => AddAsyn(Data, Binding).Wait();
        #endregion
        #region 异步方法（传入同步集合）
        public async Task AddAsyn(IEnumerable<IData> Data, bool Binding)
        {
            Data = await Task.Run(() => Data.ToArray());
            await DB.PerformSQL(DB.SQLGenerated.ScriptAdd(Data, TableName));
            Data.ForEach(x => this.Binding(x, Binding));
        }
        #endregion
        #endregion
        #endregion
        #region 关于删除数据
        #region 同步方法
        public void Delete(Expression<Func<PlaceholderData, bool>> Expression)
             => DeleteAsyn(Expression).Wait();
        #endregion
        #region 异步方法
        public Task DeleteAsyn(Expression<Func<PlaceholderData, bool>> Expression)
            => DB.PerformSQL(DB.SQLGenerated.ScripDelete(Expression, TableName));
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的数据库和表名初始化对象
        /// </summary>
        /// <param name="DB">指定的数据库</param>
        /// <param name="TableName">数据表或视图的名称</param>
        public TableOrView(DB DB, string TableName)
        {
            this.DB = DB;
            this.TableName = TableName;
            PrimaryKeyName = DB.GetPrimaryKey(TableName);
        }
        #endregion
    }
}
