namespace System.Geography.Map;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个商业单位，
/// 它表示地图上的一家店铺之类的
/// </summary>
public interface IGeographicBusiness : IGeographicInfo
{
    #region 行业
    /// <summary>
    /// 获取这个商业单位所属的行业
    /// </summary>
    string Industry { get; }
    #endregion
    #region 价格区间
    /// <summary>
    /// 获取这个商业单位的价格区间（仅供参考），
    /// 如价格不明，则为<see langword="null"/>
    /// </summary>
    IIntervalSpecific<Num>? Price { get; }
    #endregion
}
