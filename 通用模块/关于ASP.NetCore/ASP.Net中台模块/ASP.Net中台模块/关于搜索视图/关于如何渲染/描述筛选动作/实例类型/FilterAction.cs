using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是所有筛选动作的基类
/// </summary>
[JsonDerivedType(typeof(FilterActionQuery), nameof(FilterActionQuery))]
[JsonDerivedType(typeof(FilterActionSort), nameof(FilterActionSort))]
public abstract record FilterAction
{
}
