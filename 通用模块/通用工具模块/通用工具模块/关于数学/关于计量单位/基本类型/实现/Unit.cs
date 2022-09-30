namespace System.Maths;

/// <summary>
/// 这个类型是<see cref="IUnit{Template}"/>的实现，
/// 可以视为一个不可变的计量单位
/// </summary>
/// <inheritdoc cref="IUnit{Template}"/>
sealed class Unit<Template> : IUnit<Template>
    where Template : IUT
{
    #region 接口实现
    #region 关于单位的值
    #region 公制单位形式
    public Num ValueMetric { get; }
    #endregion
    #region 本单位形式
    public (Template Template, Num Value) Value { get; }
    #endregion
    #endregion
    #region 比较单位的大小
    public int CompareTo(IUnit<Template>? other)
        => ValueMetric.CompareTo(other?.ValueMetric ?? 0);
    #endregion
    #region 比较单位的相等性
    public bool Equals(IUnit<Template>? other)
        => other is { } && ValueMetric == other.ValueMetric;
    #endregion
    #endregion
    #region 重写的方法
    #region 重写GetHashCode
    public override int GetHashCode()
        => ValueMetric;
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is IUnit<Template> u && Equals(u);
    #endregion
    #region 重写ToString
    public override string ToString()
        => Value.Value.ToString() + Value.Template;
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的模板和公制单位数量初始化单位
    /// </summary>
    /// <param name="template">单位的模板</param>
    /// <param name="metricValue">公制单位的数量</param>
    public Unit(Template template, Num metricValue)
    {
        this.Value = (template, template.FromMetric(metricValue));
        this.ValueMetric = metricValue;
    }
    #endregion
}
