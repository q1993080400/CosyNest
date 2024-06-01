namespace System.DataFrancis;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以作为一个支持从数据源获取数据的管道
/// </summary>
public interface IDataPipeFrom : IDataContext, IDataPipeFromContext
{

}
