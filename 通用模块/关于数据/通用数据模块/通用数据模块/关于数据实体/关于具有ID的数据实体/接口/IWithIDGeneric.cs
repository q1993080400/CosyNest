namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个ID
/// </summary>
/// <typeparam name="IDType">ID的类型</typeparam>
public interface IWithID<IDType>
{
    #region 对象的ID
    /// <summary>
    /// 获取对象的ID，
    /// 它指的是可以作为键的ID，
    /// 一般会被用来映射到数据库主键等地方
    /// </summary>
    IDType ID { get; }
    #endregion
}
