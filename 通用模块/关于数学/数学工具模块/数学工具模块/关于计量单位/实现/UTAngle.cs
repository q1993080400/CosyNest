namespace System.MathFrancis;

/// <summary>
/// 这个类型是<see cref="IUTAngle"/>的实现，
/// 可以视为一个角度单位
/// </summary>
/// <inheritdoc cref="UT(string, Func{Num, Num}, Func{Num, Num}, bool)"/>
sealed class UTAngle(string name, Func<Num, Num> toMetric, Func<Num, Num> fromMetric) : UT(name, toMetric, fromMetric), IUTAngle
{
    #region 返回单位的类型
    protected override Type UTType
        => typeof(IUTAngle);

    #endregion
}
