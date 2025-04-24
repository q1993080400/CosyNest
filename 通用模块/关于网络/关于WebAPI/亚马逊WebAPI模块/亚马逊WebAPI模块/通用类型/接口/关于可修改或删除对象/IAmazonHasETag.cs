namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都具有一个ETag标头，它在删除或更新对象的时候会用到
/// </summary>
public interface IAmazonHasETag
{
    #region ETag标头
    /// <summary>
    /// 获取ETag标头，
    /// 它在删除或更新对象的时候会被用到
    /// </summary>
    string ETag { get; }
    #endregion
}
