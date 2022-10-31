namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IUriManager"/>的实现，
/// 可以用来管理本机Uri
/// </summary>
sealed class UriManager : IUriManager
{
    #region 获取本机Uri
    public string Host { get; init; }
    #endregion 
}
