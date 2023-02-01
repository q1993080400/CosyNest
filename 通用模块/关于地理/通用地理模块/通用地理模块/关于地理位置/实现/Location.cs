namespace System.Geography;

/// <summary>
/// 这个类型表示一个地理位置
/// </summary>
sealed class Location : ILocation
{
    #region 精度
    public required decimal Longitude { get; init; }
    #endregion
    #region 纬度
    public required decimal Latitude { get; init; }
    #endregion
    #region 精度
    public required decimal Accuracy { get; init; }
    #endregion
    #region 重写ToString
    public override string ToString()
        => $"{(Longitude >= 0 ? "东经" : "西经")}{Math.Abs(Longitude)}度；{(Latitude >= 0 ? "北纬" : "南纬")}{Math.Abs(Latitude)}度；精度{Accuracy}米";
    #endregion
}
