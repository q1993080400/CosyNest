namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性指定了如何在UI上显示表单中的布尔值
/// </summary>
public sealed class FormRenderBoolAttribute : FormRenderAttribute<FormBoolRender>
{
    #region 描述True
    /// <summary>
    /// 获取如何描述<see langword="true"/>值
    /// </summary>
    public required string DescribeTrue { get; init; }
    #endregion
    #region 描述False
    /// <summary>
    /// 获取如何描述<see langword="false"/>值
    /// </summary>
    public required string DescribeFalse { get; init; }
    #endregion
}
