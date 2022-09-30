namespace System.Maths;

/// <summary>
/// 这个类型是<see cref="IUTAngle"/>的实现，
/// 可以视为一个角度单位
/// </summary>
sealed class UTAngle : UT, IUTAngle
{
    #region 返回单位的类型
    protected override Type UTType
        => typeof(IUTAngle);
    #endregion
    #region 构造函数
    /// <inheritdoc cref="UT(string, Func{Num, Num}, Func{Num, Num}, bool)"/>
    public UTAngle(string name, Func<Num, Num> toMetric, Func<Num, Num> fromMetric)
        : base(name, toMetric, fromMetric)
    {

    }
    #endregion
}
