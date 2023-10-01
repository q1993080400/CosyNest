namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本记录是用来渲染<see cref="BusinessFormViewer{Model}"/>提交部分的参数
/// </summary>
/// <typeparam name="Model">表单模型的类型</typeparam>
public sealed record RenderBusinessFormViewerInfo<Model>
    where Model : class
{
    #region 用来提交表单的业务逻辑
    /// <summary>
    /// 获取提交表单的业务逻辑
    /// </summary>
    public required EventCallback Submit { get; init; }
    #endregion
    #region 用来重置表单的业务逻辑
    /// <summary>
    /// 获取用来重置表单的业务逻辑
    /// </summary>
    public required EventCallback Resetting { get; init; }
    #endregion
    #region 用来删除表单的业务逻辑
    /// <summary>
    /// 获取用来删除表单的业务逻辑，
    /// 如果为<see langword="null"/>，表示不支持删除
    /// </summary>
    public required EventCallback? Delete { get; init; }
    #endregion
    #region 用来取消表单的业务逻辑
    /// <summary>
    /// 用于取消表单，回到上级页面的业务逻辑，
    /// 如果为<see langword="null"/>，表示不支持取消
    /// </summary>
    public required EventCallback? Cancellation { get; set; }
    #endregion
    #region 表单模型
    /// <summary>
    /// 获取要渲染的表单模型
    /// </summary>
    public required Model FormModel { get; set; }
    #endregion
    #region 是否为现有表单
    /// <summary>
    /// 返回这个表单是否为现有表单，
    /// 现有表单支持修改和删除，不支持新增
    /// </summary>
    public required bool ExistingForms { get; init; }
    #endregion
}
