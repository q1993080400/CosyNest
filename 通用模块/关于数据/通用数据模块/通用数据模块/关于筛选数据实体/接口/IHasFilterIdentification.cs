namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个标识，
/// 它可以用来区分不同的筛选条件
/// </summary>
public interface IHasFilterIdentification
{
    #region 筛选标识
    /// <summary>
    /// 获取一个标识，
    /// 它可以用来区分不同的筛选目标
    /// </summary>
    string Identification { get; }
    #endregion
}
