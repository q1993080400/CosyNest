namespace System.NetFrancis.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个封装了序列化后数据的Http标头
/// </summary>
public interface IObjectHeaderValue
{
    #region 静态成员：标头名称
    /// <summary>
    /// 获取这个Http标头的名称
    /// </summary>
    public const string HeaderName = "CustomObject";
    #endregion
    #region 数据的Json形式
    /// <summary>
    /// 获取数据的Json形式
    /// </summary>
    string Json { get; }
    #endregion
}
