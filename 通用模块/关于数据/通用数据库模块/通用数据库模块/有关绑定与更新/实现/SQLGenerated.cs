using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Design;
using static System.DataFrancis.DB.ToolSQLGenerated;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 这个类型是<see cref="ISQLGenerated"/>的实现，
    /// 可以视为一个SQL脚本生成器
    /// </summary>
    class SQLGenerated : Singleton<SQLGenerated>, ISQLGenerated
    {
        #region 生成查询数据的脚本
        public string ScriptInquire(Expression<Func<PlaceholderData, bool>> Expression, string TableName)
            => $"select * from {TableName} where {ToWhere(Expression)}";
        #endregion
        #region 生成添加数据的脚本
        public string ScriptAdd(IEnumerable<IData> Datas, string TableName)
        {
            #region 本地函数
            static string Get(IEnumerable<object?> List)
                => List.Join(",");
            #endregion
            var arry = Datas.ToArray();
            return $"insert into {TableName} ({Get(arry[0].Keys)}) " +
                $"values {Get(arry.Select(x => "(" + Get(x.Values) + ")"))}";
        }
        #endregion
        #region 生成更新数据的脚本
        public string ScripUpdate(string ColumnName, object? NewValue, string PrimaryKeyName, object PrimaryKey, string TableName)
            => $"update {TableName} set {ColumnName}={ToSQLText(NewValue)} where {PrimaryKeyName}={ToSQLText(PrimaryKey)}";
        #endregion
        #region 生成删除数据的脚本
        #region 单独删除
        public string ScripDelete(string PrimaryKeyName, object PrimaryKey, string TableName)
            => $"delete from {TableName} where {PrimaryKeyName}={ToSQLText(PrimaryKey)}";
        #endregion
        #region 批量删除
        public string ScripDelete(Expression<Func<PlaceholderData, bool>> Expression, string TableName)
            => $"delete from {TableName} where {ToWhere(Expression)}";
        #endregion
        #endregion
        #region 构造函数
        private SQLGenerated()
        {

        }
        #endregion
    }
}
