namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性允许根据生日对年龄进行验证
/// </summary>
public sealed class VerifyAgeAttribute : VerifyAttribute
{
    #region 公开成员
    #region 最大年龄
    /// <summary>
    /// 获取所允许的最大年龄
    /// </summary>
    public double MaxAge { get; init; } = 255;
    #endregion
    #region 最小年龄
    /// <summary>
    /// 获取所允许的最小年龄
    /// </summary>
    public double MinAge { get; init; } = 0;
    #endregion
    #region 验证对象
    public override string? Verify(object? obj, string describe)
    {
        if (obj is not DateTimeOffset birthday)
            return $"{describe}不是一个日期，验证不通过";
        var age = (double)birthday.TotalYears();
        return age < 0 ?
            $"在当前时间，当事人还没有出生，验证不通过" :
            IInterval.CheckInInterval(age, MinAge, MaxAge) switch
            {
                IntervalPosition.Overflow => $"年龄大于所允许的最大值{MaxAge}，验证不通过",
                IntervalPosition.Insufficient => $"年龄小于所允许的最小值{MinAge}，验证不通过",
                _ => null
            };
    }
    #endregion
    #endregion
}
