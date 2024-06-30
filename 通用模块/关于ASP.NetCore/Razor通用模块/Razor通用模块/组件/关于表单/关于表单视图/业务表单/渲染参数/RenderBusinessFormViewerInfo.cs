namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本记录是用来渲染<see cref="BusinessFormViewer{Model}"/>提交部分的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public sealed record RenderBusinessFormViewerInfo<Model>
    where Model : class
{
    #region 用来提交表单的业务逻辑
    /// <summary>
    /// 获取提交表单的业务逻辑
    /// </summary>
    public required Func<Task> Submit { get; init; }
    #endregion
    #region 用来删除表单的业务逻辑
    /// <summary>
    /// 获取用来删除表单的业务逻辑，
    /// 如果为<see langword="null"/>，表示不支持删除
    /// </summary>
    public required Func<Task>? Delete { get; init; }
    #endregion
    #region 用来取消表单的业务逻辑
    /// <summary>
    /// 用于取消表单，回到上级页面的业务逻辑，
    /// 如果为<see langword="null"/>，表示不支持取消
    /// </summary>
    public required Func<Task>? Cancellation { get; set; }
    #endregion
    #region 基础版本的渲染参数
    /// <summary>
    /// 获取基础版本的渲染参数
    /// </summary>
    public required RenderSubmitInfo<Model> BaseRenderInfo { get; init; }
    #endregion
}
