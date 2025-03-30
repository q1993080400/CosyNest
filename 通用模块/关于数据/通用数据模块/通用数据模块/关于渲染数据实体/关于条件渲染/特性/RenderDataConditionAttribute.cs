namespace System.DataFrancis;

/// <summary>
/// 这个组件指示渲染表单属性的条件
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class RenderDataConditionAttribute : Attribute
{
    #region 渲染条件
    /// <summary>
    /// 获取渲染条件的名称，
    /// 它可以让某些属性仅在某些情况下被渲染
    /// </summary>
    public required string RenderCondition { get; init; }
    #endregion
}
