namespace System.NetFrancis;

/// <summary>
/// 这个类型是<see cref="IHostProvide"/>的实现，
/// 可以用来提供本机Host地址
/// </summary>
sealed class HostProvide : IHostProvide
{
    #region 获取本机Uri
    public required UriHost Host { get; init; }
    #endregion
}
