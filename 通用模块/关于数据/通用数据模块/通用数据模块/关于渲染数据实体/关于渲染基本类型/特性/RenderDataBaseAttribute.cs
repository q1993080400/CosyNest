namespace System.DataFrancis;

/// <summary>
/// 这个类型是数据渲染特性的基类
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
public abstract class RenderDataBaseAttribute : Attribute
{
    #region 分组的名字
    /// <summary>
    /// 获取分组的名字，
    /// 同一组的属性在一起渲染
    /// </summary>
    public string? GroupName { get; init; }
    #endregion
    #region 顺序
    /// <summary>
    /// 显式指定要显示的顺序
    /// </summary>
    public int Order { get; init; }
    #endregion
    #region 是否仅显示
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示仅提供数据显示功能，不提供数据编辑功能
    /// </summary>
    public bool IsReadOnly { get; init; }
    #endregion
}
