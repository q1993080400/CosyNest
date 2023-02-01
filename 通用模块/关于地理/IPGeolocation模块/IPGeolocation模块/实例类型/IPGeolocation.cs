using System.Net;

using IP2Location;

namespace System.Geography;

/// <summary>
/// 这个类型可以通过IP2Location.IPGeolocation对IP地址进行定位
/// </summary>
sealed class IPGeolocation : IDisposable
{
    #region 公开成员
    #region 执行定位
    /// <inheritdoc cref="Geography.IPLocation"/>
    public Task<IAdministrativeArea?> IPLocation(IPAddress address)
    {
        var result = Location.IPQuery(address.ToString());
        var area = result is { Status: "OK", CountryLong: not "-" } ?
            CreateGeography.Area(new[] { result.CountryLong, result.Region, result.City }) : null;
        return Task.FromResult(area);
    }
    #endregion
    #region 释放对象
    public void Dispose()
    {
        Location.Close();
    }
    #endregion
    #endregion
    #region 内部成员
    #region IP定位器对象
    /// <summary>
    /// 获取IP定位器对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private Component Location { get; } = new();
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="dbPath">程序会通过这个路径加载IP位置数据库，必须为bin格式</param>
    public IPGeolocation(string dbPath)
    {
        if (!File.Exists(dbPath))
            throw new NotSupportedException($"为了正确地获取IP地址的位置，" +
                $"程序需要从路径{Path.GetFullPath(dbPath)}加载离线数据库，但是这个路径不存在");
        Location.Open(dbPath, true);
    }
    #endregion
}
