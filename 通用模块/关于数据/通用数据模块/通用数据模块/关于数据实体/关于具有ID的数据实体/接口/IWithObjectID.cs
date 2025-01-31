namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个对象ID
/// </summary>
public interface IWithObjectID
{
    #region 对象的ID
    /// <summary>
    /// 获取对象的ID，
    /// 它不映射到数据库主键等任何地方，
    /// 它只是这个Net对象本身的ID
    /// </summary>
    Guid ObjectID { get; }
    #endregion
}
