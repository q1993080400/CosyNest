namespace System.Geography;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来表示一个地理位置
/// </summary>
public interface ILocation
{
    #region 经度
    /// <summary>
    /// 返回经度，
    /// 正值表示东经，负值表示西经
    /// </summary>
    decimal Longitude { get; }
    #endregion
    #region 纬度
    /// <summary>
    /// 返回纬度，
    /// 正值表示北纬，负值表示南纬
    /// </summary>
    decimal Latitude { get; }
    #endregion
}
