namespace System.IOFrancis;

/// <summary>
/// 凡是实现这个接口的类型
/// 都可以作为一个流适配器，
/// 它可以以变通的方式，
/// 让只支持异步读取的流能够同步读取
/// </summary>
public interface IStreamAdapter
{
    #region 返回同步流
    /// <summary>
    /// 如果当前流可以同步读取，
    /// 则返回它本身，否则返回一个可以同步读取的流
    /// </summary>
    /// <returns></returns>
    Task<Stream> SynchronousAdapter();
    #endregion
}
