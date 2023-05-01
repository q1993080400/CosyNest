namespace Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// 这个静态类是有关SignalR的工具类
/// </summary>
public static class ToolSignalR
{
    #region 返回强类型Hub中心的默认Uri
    /// <summary>
    /// 如果一个Hub是强类型中心，
    /// 则返回它的默认Uri
    /// </summary>
    /// <typeparam name="Interface">Hub中心实现的接口的类型</typeparam>
    /// <returns></returns>
    public static string GetHubDefaultUri<Interface>()
        where Interface : class
        => "/Hub/" + typeof(Interface).Name.TrimStart('I');
    #endregion
}
