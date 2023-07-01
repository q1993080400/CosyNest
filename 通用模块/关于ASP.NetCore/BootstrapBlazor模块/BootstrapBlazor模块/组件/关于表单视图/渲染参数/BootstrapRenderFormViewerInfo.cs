using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本记录是用来渲染<see cref="BootstrapFormViewer{Model}"/>的参数
/// </summary>
/// <inheritdoc cref="RenderFormViewerInfo{Model}"/>
public sealed record BootstrapRenderFormViewerInfo<Model>
    where Model : class, new()
{
    #region 基础渲染参数
    /// <summary>
    /// 获取基础版本的渲染参数
    /// </summary>
    public required RenderFormViewerInfo<Model> RenderFormViewerInfo { get; init; }
    #endregion
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
    /// 获取用来删除表单的业务逻辑
    /// </summary>
    public required EventCallback Delete { get; init; }
    #endregion
}
