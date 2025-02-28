namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个拥有排序顺序的实体
/// </summary>
public interface IHasSort
{
    #region 对象的顺序
    /// <summary>
    /// 获取对象的顺序，
    /// 按升序排序
    /// </summary>
    int Sort { get; }
    #endregion
}
