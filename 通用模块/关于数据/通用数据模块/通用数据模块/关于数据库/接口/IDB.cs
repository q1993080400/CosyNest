namespace System.DataFrancis.DB;

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

      问：为什么本接口只支持通过视图和表查询数据？
      答：因为视图和表不仅能够获取数据，而且能够为更新数据提供方便，
      举例说明，如果一个数据是通过存储过程获取的，那么根本不知道它应该如何更新
      同时，作者认为数据库应该只作为一个提供数据的接口，客户端接收数据即可，
      不需要也不应该使用过于复杂的功能*/
    #endregion
    #region 获取表
    /// <summary>
    /// 根据名称或表达式，获取表
    /// </summary>
    /// <param name="tableExpression">表的名称或表达式</param>
    /// <returns>具有指定名称的表或视图</returns>
    IDataPipe GetTable(string tableExpression);

    /*问：如何使用本API表示表连接？
      答：tableExpression参数既可以是表或视图的名称，
      也可以是一个表表达式，语法与SQL相同，
      当它是表达式的时候，可以用来表示表连接，例如：
     
      SELECT p.Name, sod.SalesOrderID  
      FROM Production.Product AS p  
      INNER JOIN Sales.SalesOrderDetail AS sod  
      ON p.ProductID = sod.ProductID  
    
      如果tableExpression参数以”select “开头，则将其视为表达式，
      否则将其视为表的名称，判断表达式不区分大小写，且包括后面的那个空格，这是为了避免误判
    
      表达式仅用于返回连接后的虚拟表，它不包括where子句及后面的部分*/
    #endregion
    #region 直接执行SQL脚本
    /// <summary>
    /// 直接执行SQL脚本，
    /// 并以<see cref="IData"/>的形式返回结果
    /// </summary>
    /// <typeparam name="Data">返回值类型</typeparam>
    /// <param name="script">待执行的SQL脚本</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns>查询到的数据，如果不是查询操作，则返回一个空数据</returns>
    Task<Data[]> ExecuteScript<Data>(string script, CancellationToken cancellationToken = default)
        where Data : IData;
    #endregion
}
