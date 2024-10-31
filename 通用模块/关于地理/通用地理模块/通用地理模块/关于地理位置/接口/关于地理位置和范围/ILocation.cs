namespace System.Geography;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来表示一个地理位置
/// </summary>
public interface ILocation
{
    #region 对位置的描述
    /// <summary>
    /// 获取对位置的描述
    /// </summary>
    string? Description { get; }
    #endregion
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
    #region 精度
    /// <summary>
    /// 获取定位的精度，以米为单位
    /// </summary>
    decimal Accuracy { get; }
    #endregion
    #region 解构对象
    /// <summary>
    /// 将对象解构为经度，纬度，和精度
    /// </summary>
    /// <param name="longitude">经度</param>
    /// <param name="latitude">纬度</param>
    /// <param name="accuracy">精度</param>
    void Deconstruct(out decimal longitude, out decimal latitude, out decimal accuracy)
    {
        longitude = Longitude;
        latitude = Latitude;
        accuracy = Accuracy;
    }
    #endregion
}
