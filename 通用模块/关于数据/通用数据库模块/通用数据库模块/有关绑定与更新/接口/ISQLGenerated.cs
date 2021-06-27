using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Concurrent;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个SQL脚本生成引擎，
    /// 它可以告诉<see cref="IDB"/>如何解释表达式并生成SQL脚本
    /// </summary>
    public interface ISQLGenerated
    {
        #region 缓存特殊成员
        /// <summary>
        /// 缓存具有特殊意义的类型成员，
        /// 它们在表达式树中不会被求值，
        /// 而是使用自定义的方式解释为SQL脚本
        /// </summary>
        public static IAddOnlyDictionary<MemberInfo, Func<Expression, string>> SpecialMethod { get; }
        = CreateEnumerable.AddOnlyDictionary(new ConcurrentDictionary<MemberInfo, Func<Expression, string>>());

        /*说明文档：
          如果需要对属性进行特殊解释，
          可以把它的Get或Set方法注册进这个字典中*/
        #endregion
        #region 有关生成脚本
        #region 生成查询数据的脚本
        /// <summary>
        /// 解析表达式，并返回用于查询数据的SQL脚本
        /// </summary>
        /// <param name="Expression">待解析的查询表达式</param>
        /// <param name="TableName">数据表或视图的名称</param>
        /// <returns>用于查询数据的SQL脚本</returns>
        string ScriptInquire(Expression<Func<PlaceholderData, bool>> Expression, string TableName);
        #endregion
        #region 生成添加数据的脚本
        /// <summary>
        /// 根据要添加的数据，返回用于添加数据的SQL脚本
        /// </summary>
        /// <param name="Datas">待添加的数据</param>
        /// <param name="TableName">数据表或视图的名称</param>
        /// <returns>用于添加数据的SQL脚本</returns>
        string ScriptAdd(IEnumerable<IData> Datas, string TableName);
        #endregion
        #region 生成更新数据的脚本
        /// <summary>
        /// 根据发生更改的数据，返回用于更新数据的脚本
        /// </summary>
        /// <param name="ColumnName">发生修改的列名</param>
        /// <param name="NewValue">数据的新值</param>
        /// <param name="PrimaryKeyName">数据主键的列名</param>
        /// <param name="PrimaryKey">数据的主键</param>
        /// <param name="TableName">数据表或视图的名称</param>
        /// <returns>用于更新数据的脚本</returns>
        string ScripUpdate(string ColumnName, object? NewValue, string PrimaryKeyName, object PrimaryKey, string TableName);
        #endregion
        #region 生成删除数据的脚本
        #region 单独删除
        /// <summary>
        /// 生成用于删除单条数据的脚本
        /// </summary>
        /// <param name="PrimaryKeyName">数据主键的列名</param>
        /// <param name="PrimaryKey">数据的主键</param>
        /// <param name="TableName">数据表的名称</param>
        /// <returns>删除单条数据的脚本</returns>
        string ScripDelete(string PrimaryKeyName, object PrimaryKey, string TableName);
        #endregion
        #region 批量删除
        /// <summary>
        /// 生成用于批量删除数据的脚本
        /// </summary>
        /// <param name="Expression">用来指定删除条件的谓词</param>
        /// <param name="TableName">数据表的名称</param>
        /// <returns>用于删除所有符合指定谓词数据的脚本</returns>
        string ScripDelete(Expression<Func<PlaceholderData, bool>> Expression, string TableName);
        #endregion
        #endregion
        #endregion 
        #region 静态构造函数
        static ISQLGenerated()
        {
            #region 用于获取数据列名的本地函数
            static string Get(Expression m)
                => m.ToOt<MethodCallExpression>().Arguments[0].CalValue()?.ToString() ??
                throw new NullReferenceException("列名的返回值不能为null");
            #endregion
            SpecialMethod.Add(PlaceholderData.Indexer, Get);
        }
        #endregion
    }
}
