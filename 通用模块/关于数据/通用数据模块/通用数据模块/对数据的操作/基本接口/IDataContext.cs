namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个数据上下文，
/// 来自同一上下文的数据可以进行表连接
/// </summary>
public interface IDataContext : IAsyncDisposable
{
}
