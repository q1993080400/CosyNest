namespace Microsoft.AspNetCore;

/// <summary>
/// 描述要筛选的对象的类型
/// </summary>
public enum FilterObjectType
{
    /// <summary>
    /// 字符串
    /// </summary>
    Text,
    /// <summary>
    /// 数字
    /// </summary>
    Num,
    /// <summary>
    /// 日期
    /// </summary>
    Date,
    /// <summary>
    /// 枚举
    /// </summary>
    Enum,
    /// <summary>
    /// 布尔值
    /// </summary>
    Bool
}
