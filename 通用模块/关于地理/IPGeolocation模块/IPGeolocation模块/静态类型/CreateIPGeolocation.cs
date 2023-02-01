namespace System.Geography;

/// <summary>
/// 这个静态类可以用来创建和IP离线定位有关的对象
/// </summary>
public static class CreateIPGeolocation
{
    #region 创建IP离线定位委托
    /// <summary>
    /// 返回一个元组，它的第一个项是一个可以通过IP执行离线定位的委托，
    /// 第二个项是执行释放非托管资源的委托
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IPGeolocation(string)"/>
    public static (IPLocation Location, Action Disposable) IPLocation(string dbPath = "IPGeolocationDB.bin")
    {
        var ipGeolocation = new IPGeolocation(dbPath);
        return (ipGeolocation.IPLocation, ipGeolocation.Dispose);
    }

    /*提示：
      请在这个地址下载离线数据库
      https://lite.ip2location.com/ip2location-lite
      */
    #endregion
}
