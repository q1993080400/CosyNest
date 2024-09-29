namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个创建时间
/// </summary>
public interface IWithDate
{
    #region 创建时间
    /// <summary>
    /// 获取本对象的创建时间
    /// </summary>
    DateTimeOffset Date { get; }
    #endregion
}
