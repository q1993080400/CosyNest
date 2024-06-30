using System.DataFrancis;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是所有筛选目标的基类
/// </summary>
[JsonDerivedType(typeof(FilterTargetMultiple), nameof(FilterTargetMultiple))]
[JsonDerivedType(typeof(FilterTargetSingle), nameof(FilterTargetSingle))]
public abstract record FilterTarget : IHasFilterTarget
{
    #region 类型
    /// <summary>
    /// 描述要筛选的对象的类型
    /// </summary>
    public required FilterObjectType FilterObjectType { get; init; }
    #endregion
    #region 枚举的值
    /// <summary>
    /// 如果要筛选枚举，
    /// 则这个集合描述枚举可能的值和描述
    /// </summary>
    public required IReadOnlyCollection<EnumItem> EnumItem { get; init; }
    #endregion
    #region 是否存在自定义默认值
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示存在自定义默认值，需要进行处理
    /// </summary>
    public abstract bool HasDefaultValue { get; }
    #endregion
    #region 筛选标识
    public abstract string Identification { get; }
    #endregion
    #region 是否为虚拟筛选
    public required bool IsVirtually { get; init; }
    #endregion
}
