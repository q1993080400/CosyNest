using System.Net;

namespace System.Geography;

/// <summary>
/// 根据IP地址，获取它的位置
/// </summary>
/// <param name="address">要获取位置的IP地址</param>
/// <returns>IP地址的位置，如果获取失败，或者是内网IP，则返回<see langword="null"/></returns>
public delegate Task<IAdministrativeArea?> IPLocation(IPAddress address);