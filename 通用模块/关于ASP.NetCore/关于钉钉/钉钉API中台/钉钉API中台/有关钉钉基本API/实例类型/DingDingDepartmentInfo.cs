namespace System.DingDing;

/// <summary>
/// 这个记录表示钉钉部门
/// </summary>
public sealed record DingDingDepartmentInfo
{
    #region 部门名称
    /// <summary>
    /// 返回部门名称
    /// </summary>
    public required string Name { get; init; }
    #endregion
    #region 部门ID
    /// <summary>
    /// 部门ID
    /// </summary>
    public required string ID { get; init; }
    #endregion
    #region 子部门
    /// <summary>
    /// 获取所有子部门
    /// </summary>
    public required IReadOnlyCollection<DingDingDepartmentInfo> Son { get; init; }
    #endregion
    #region 获取所有部门成员
    /// <summary>
    /// 获取部门的所有成员
    /// </summary>
    public required IReadOnlyCollection<DingDingDepartmentRole> Member { get; init; }
    #endregion
    #region 递归获取所有部门
    /// <summary>
    /// 递归获取所有部门
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DingDingDepartmentInfo> SonAllDepartment()
    {
        yield return this;
        foreach (var son in Son)
        {
            foreach (var grandson in son.SonAllDepartment())
            {
                yield return grandson;
            }
        }
    }
    #endregion
    #region 递归获取所有成员
    /// <summary>
    /// 递归获取所有成员
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DingDingDepartmentRole> SonAllMember()
    {
        foreach (var department in SonAllDepartment())
        {
            foreach (var member in department.Member)
            {
                yield return member;
            }
        }
    }
    #endregion
}
