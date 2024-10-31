namespace System.Geography;

/// <summary>
/// 这个静态类可以用来创建和地理有关的对象
/// </summary>
public static class CreateGeography
{
    #region 创建地理位置
    /// <summary>
    /// 创建一个地理位置对象
    /// </summary>
    /// <param name="longitude">经度，正值表示东经，负值表示西经</param>
    /// <param name="latitude">纬度，正值表示北纬，负值表示南纬</param>
    /// <param name="description">对位置的描述</param>
    /// <param name="accuracy">定位的精度，以米为单位</param>
    public static ILocation Location(decimal longitude, decimal latitude, string? description = null, decimal accuracy = 50)
        => new Location
        {
            Longitude = longitude,
            Latitude = latitude,
            Accuracy = accuracy,
            Description = description
        };
    #endregion
}
