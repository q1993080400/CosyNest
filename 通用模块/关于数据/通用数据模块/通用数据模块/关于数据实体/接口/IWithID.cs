namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个ID
/// </summary>
public interface IWithID
{
    #region 数据的ID
    /// <summary>
    /// 获取数据的ID
    /// </summary>
    public Guid ID { get; }
    #endregion
}
