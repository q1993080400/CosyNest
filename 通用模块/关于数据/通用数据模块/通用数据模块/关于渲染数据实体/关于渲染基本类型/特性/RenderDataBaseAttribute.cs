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
    #region 指示如何判断属性是否可以编辑
    /// <summary>
    /// 指示如何判断这个属性是否可以编辑
    /// </summary>
    public CanEditMod CanEditMod { get; init; }
    #endregion
}
