using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 这个类型是<see cref="IDataBinding"/>的实现，
    /// 可以为数据库提供绑定服务
    /// </summary>
    class DBBinding : IDataBinding
    {
        #region 帮助实现的底层成员
        #region 数据库对象
        /// <summary>
        /// 获取负责和数据库进行通讯的对象
        /// </summary>
        private IDB DB { get; }
        #endregion
        #region 数据表名称
        /// <summary>
        /// 获取数据表的名称
        /// </summary>
        private string TableName { get; }
        #endregion
        #region 获取主键的名称
        /// <summary>
        /// 获取主键的列名
        /// </summary>
        private string PrimaryKeyName { get; }
        #endregion
        #region 获取主键的值
        /// <summary>
        /// 获取主键的值
        /// </summary>
        private object PrimaryKey { get; }
        #endregion
        #endregion
        #region 数据通知数据源
        #region 通知修改
        public async void NoticeUpdateToSource(string ColumnName, object? NewValue)
            => await DB.PerformSQL(DB.SQLGenerated.
                ScripUpdate(ColumnName, NewValue, PrimaryKeyName, PrimaryKey, TableName));
        #endregion
        #region 通知删除
        public async void NoticeDeleteToSource()
        {
            await DB.PerformSQL(DB.SQLGenerated.
                ScripDelete(PrimaryKeyName, PrimaryKey, TableName));
        }
        #endregion
        #endregion
        #region 数据源通知数据
        #region 通知修改
        public event Action<string, object?>? NoticeUpdateToData
        {
            add { }
            remove { }
        }
        #endregion
        #region 通知删除
        public event Action? NoticeDeleteToData
        {
            add { }
            remove { }
        }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="DB">负责和数据库进行通讯的对象</param>
        /// <param name="TableName">数据表的名称</param>
        /// <param name="PrimaryKeyName">主键的列名</param>
        /// <param name="PrimaryKey">主键的值</param>
        public DBBinding(IDB DB, string TableName, string PrimaryKeyName, object PrimaryKey)
        {
            this.DB = DB;
            this.TableName = TableName;
            this.PrimaryKeyName = PrimaryKeyName;
            this.PrimaryKey = PrimaryKey;
        }
        #endregion
    }
}
