namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染表单视图提交部分的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderSubmitInfo<Model>
    where Model : class
{
    #region 获取所有属性是否全部为只读
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个表单中的所有属性均为只读，
    /// 它一般情况下不显示提交按钮，但是也有例外
    /// </summary>
    public required bool AllReadOnly { get; init; }
    #endregion
    #region 用来重置表单的委托
    /// <summary>
    /// 调用这个委托可以重置表单，
    /// 注意：它仅重置模型，
    /// 不包含其他业务逻辑
    /// </summary>
    public required Func<Task> Resetting { get; init; }
    #endregion
    #region 是否为现有表单
    /// <summary>
    /// 返回这个表单是否为现有表单，
    /// 现有表单支持修改和删除，不支持新增
    /// </summary>
    public required bool ExistingForms { get; init; }
    #endregion
    #region 表单模型
    /// <summary>
    /// 获取要渲染的表单模型
    /// </summary>
    public required Model FormModel { get; set; }
    #endregion
}
