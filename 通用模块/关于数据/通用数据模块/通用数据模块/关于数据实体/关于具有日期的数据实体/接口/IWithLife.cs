namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个具有寿命的数据实体，
/// 它可以指示实体是否已经过期
/// </summary>
public interface IWithLife
{
    #region 是否过期
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个实体已经过期，否则尚未过期
    /// </summary>
    bool IsExpire { get; }
    #endregion
}
