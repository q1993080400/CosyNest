namespace System.Geography;

/// <summary>
/// 这个记录表示一个行政区划
/// </summary>
sealed record AdministrativeArea : IAdministrativeArea
{
    #region 行政区
    public IReadOnlyList<string> Area { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="area">行政区以及它的所有上级</param>
    public AdministrativeArea(IReadOnlyList<string> area)
    {
        Area = area;
    }
    #endregion
}
