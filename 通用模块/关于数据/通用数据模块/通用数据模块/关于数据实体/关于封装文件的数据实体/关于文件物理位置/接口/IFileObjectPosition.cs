namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个文件的物理位置，
/// 它指示这个文件实际存储的地方，
/// 并具有一个ID，可以和数据库中储存文件的对象关联起来
/// </summary>
public interface IFileObjectPosition : IFilePosition, IWithID
{
}
