using System.Design;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个数据上下文，
/// 来自同一上下文的数据可以进行表连接，
/// 而且共享事务，只有在释放上下文的时候，才会保存上下文所做的更改
/// </summary>
public interface IDataContext : IInstruct, IDisposable
{
}
