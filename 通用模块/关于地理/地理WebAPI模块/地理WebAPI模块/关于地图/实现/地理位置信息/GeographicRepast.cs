using System.Maths;

namespace System.Geography.Map;

/// <summary>
/// 这个类型是<see cref="IGeographicRepast"/>的实现，
/// 可以视为一个地图上的餐饮商家
/// </summary>
sealed class GeographicRepast : IGeographicRepast
{
    #region 行业
    public string Industry => "餐饮";
    #endregion
    #region 名称
    public string Name { get; init; }
    #endregion
    #region 价格区间
    private readonly IIntervalSpecific<Num>? PriceField;

    public IIntervalSpecific<Num>? Price
    {
        get => PriceField;
        init
        {
            if (value is { } && !value.IsClosed)
                throw new NotSupportedException("价格必须是一个闭区间");
            PriceField = value;
        }
    }
    #endregion
    #region 所在位置
    public ILocation Location { get; init; }
    #endregion
    #region 距离
    public IUnit<IUTLength>? Distance { get; init; }
    #endregion
}
