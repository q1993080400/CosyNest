namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个Guid类型的ID
/// </summary>
public interface IWithID : IWithID<Guid>
{
}
