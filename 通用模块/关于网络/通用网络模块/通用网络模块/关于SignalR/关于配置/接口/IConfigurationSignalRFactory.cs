using Microsoft.AspNetCore.SignalR.Client;

namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来配置SignalR工厂
/// </summary>
public interface IConfigurationSignalRFactory
{
    #region 配置SignalR工厂
    /// <summary>
    /// 配置SignalR工厂，
    /// 并返回可用于继续配置的<see cref="IHubConnectionBuilder"/>
    /// </summary>
    /// <param name="uri">中心的地址，
    /// 它可以为相对地址，也可以为绝对地址</param>
    /// <returns></returns>
    Task<IHubConnectionBuilder> Builder(string uri);
    #endregion
    #region 转换中心地址
    /// <summary>
    /// 将中心的相对地址转换为绝对地址，
    /// 如果它已经是绝对地址，则不进行转换
    /// </summary>
    /// <param name="uri">待转换的地址</param>
    /// <returns></returns>
    string ToAbsoluteUri(string uri);
    #endregion
}
