using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 本记录是用来渲染<see cref="BootstrapFormViewer{Model}"/>提交部分的参数
/// </summary>
public sealed record BootstrapRenderSubmitInfo
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
}
