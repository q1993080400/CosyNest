namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个ID
/// </summary>
public interface IWithID
{
    #region 对象的ID
    /// <summary>
    /// 获取对象的ID
    /// </summary>
    Guid ID { get; }
    #endregion
}
