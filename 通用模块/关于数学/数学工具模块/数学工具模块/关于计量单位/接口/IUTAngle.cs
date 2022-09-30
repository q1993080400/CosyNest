namespace System.Maths;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个角度单位
/// </summary>
public interface IUTAngle : IUT
{
    #region 预设单位
    #region 角度（公制单位）
    /// <summary>
    /// 返回代表角度的单位，这是本单位的公制单位
    /// </summary>
    public static IUTAngle AngleMetric { get; }
    = new UTAngle("角度", x => x, x => x);
    #endregion
    #region 弧度
    #region 弧度常量
    /// <summary>
    /// 这个常量代表1度的角所对应的弧度
    /// </summary>
    private const decimal Corresponding = (decimal)Math.PI / 180;
    #endregion
    #region 正式属性
    /// <summary>
    /// 返回代表弧度的单位
    /// </summary>
    public static IUTAngle Radian { get; }
    = new UTAngle("弧度",
        x => x / Corresponding,
        x => x * Corresponding);
    #endregion
    #endregion
    #endregion
    #region 创建单位
    /// <inheritdoc cref="IUTLength.Create(string, Func{Num, Num}, Func{Num, Num}, bool)"/>
    public static IUTAngle Create(string name, Func<Num, Num> toMetric, Func<Num, Num> fromMetric)
        => new UTAngle(name, toMetric, fromMetric);
    #endregion
}
