using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个数据库
    /// </summary>
    public interface IDB
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #本接口没有声明有关连接的API，
          因此要求程序必须自动管理这些细节
          
          #本接口的所有API应该都没有副作用，
          因为很可能在多个线程中同时调用它们
          
          #当直接执行SQL脚本时，默认应启用事务
            
          问：为什么本接口只支持通过视图和表查询数据？
          答：因为视图和表不仅能够获取数据，而且能够为更新数据提供方便，
          举例说明，如果一个数据是通过存储过程获取的，那么根本不知道它应该如何更新
          同时，作者认为数据库应该只作为一个提供数据的接口，客户端接收数据即可，
          不需要也不应该使用过于复杂的功能*/
        #endregion
        #region 获取主键
        /// <summary>
        /// 获取指定表或视图的主键
        /// </summary>
        /// <param name="Name">要获取主键的表或视图的名称</param>
        /// <returns>主键的列名，如果没有主键，则返回<see langword="null"/>，
        /// 没有主键的表或视图不支持更新</returns>
        string? GetPrimaryKey(string Name);
        #endregion
        #region 用于生成SQL脚本的对象
        /// <summary>
        /// 获取用于生成SQL脚本的对象
        /// </summary>
        ISQLGenerated SQLGenerated { get; }
        #endregion
        #region 执行SQL脚本
        /// <summary>
        /// 执行SQL脚本
        /// </summary>
        /// <param name="Script">要执行的SQL脚本</param>
        /// <returns>执行脚本后获取到的查询结果</returns>
        Task<IData[]> PerformSQL(string Script);
        #endregion
        #region 获取表或视图
        /// <summary>
        /// 根据名称，获取表或视图
        /// </summary>
        /// <param name="Name">表或视图的名称</param>
        /// <returns>具有指定名称的表或视图</returns>
        IDataPipe GetTableOrView(string Name);
        #endregion
    }
}
