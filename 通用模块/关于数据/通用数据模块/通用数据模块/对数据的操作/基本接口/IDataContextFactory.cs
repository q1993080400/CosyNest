namespace System.DataFrancis;

/// <summary>
/// 这个类型是可以用来创建数据上下文的工厂
/// </summary>
/// <typeparam name="Context">要创建的数据上下文的类型</typeparam>
public interface IDataContextFactory<out Context>
    where Context : IDataContext
{
    #region 创建数据上下文
    /// <summary>
    /// 创建一个新的数据上下文
    /// </summary>
    /// <returns></returns>
    Context CreateContext();
    #endregion 
}
