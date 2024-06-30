namespace System.DataFrancis;

/// <summary>
/// 表示一个具有期限的数据实体，
/// 它可以区分已过期和未过期
/// </summary>
public interface IWithTerm : IWithDate
{
    #region 是否未过期
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个实体尚未过期，否则表示已经过期
    /// </summary>
    bool IsUnexpired { get; }
    #endregion
}
