namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个有效期
/// </summary>
public interface IWithTerm
{
    #region 对象的有效期
    /// <summary>
    /// 获取对象的有效期限，
    /// 如果当前时间晚于这个期限，
    /// 表示对象已经失效
    /// </summary>
    DateTimeOffset Life { get; }
    #endregion
}
