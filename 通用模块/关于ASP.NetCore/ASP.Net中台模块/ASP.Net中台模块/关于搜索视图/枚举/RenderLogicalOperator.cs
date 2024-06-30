namespace Microsoft.AspNetCore;

/// <summary>
/// 获取用来渲染的逻辑运算符
/// </summary>
public enum RenderLogicalOperator
{
    /// <summary>
    /// 表示不能显式指定逻辑运算符
    /// </summary>
    None,
    /// <summary>
    /// 等于
    /// </summary>
    Equal,
    /// <summary>
    /// 不等于
    /// </summary>
    NotEqual,
    /// <summary>
    /// 包含
    /// </summary>
    Contain
}
