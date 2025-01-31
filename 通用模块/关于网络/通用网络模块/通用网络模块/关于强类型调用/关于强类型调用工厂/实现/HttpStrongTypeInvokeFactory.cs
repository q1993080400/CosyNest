namespace System.NetFrancis;

/// <summary>
/// 这个类型是<see cref="IStrongTypeInvokeFactory"/>的实现，
/// 可以用来创建强类型Http调用
/// </summary>
/// <param name="httpClient">待封装的Http客户端</param>
sealed class HttpStrongTypeInvokeFactory(IHttpClient httpClient) : IStrongTypeInvokeFactory
{
    #region 创建强类型调用
    public IStrongTypeInvoke<API> StrongType<API>()
        where API : class
        => new HttpStrongTypeInvoke<API>(httpClient);
    #endregion
}
