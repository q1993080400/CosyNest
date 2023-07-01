namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性指定了如何在UI上显示表单中的文本
/// </summary>
public sealed class FormRenderStringAttribute : FormRenderAttribute<FormStringRender>
{
    #region 文本行数
    /// <summary>
    /// 如果处于长文本渲染模式，
    /// 则指定文本的最大行数，否则无效
    /// </summary>
    public int Rows { get; init; } = 4;
    #endregion
}
