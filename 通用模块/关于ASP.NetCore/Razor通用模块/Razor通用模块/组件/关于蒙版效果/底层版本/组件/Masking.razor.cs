namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件能够让放置在其中的子内容自动获得蒙版效果
/// </summary>
public sealed partial class Masking : ComponentBase, IContentComponent<RenderFragment<RenderMasking>>, IMaskingParameter
{
    #region 如何使用本组件
    /*请按照以下步骤添加蒙版效果：
      
      1.在子内容中添加一个外层容器标签，一般是div，
      把RenderMasking.ExternalCSS赋值给它的class
    
      2.在子内容中添加一个内部容器标签，一般是div，
      把RenderMasking.InternalCSS赋值给它的class，
      如果InternalCSS为null，不需要执行这个操作
    
      3.设置外层容器的背景样式*/
    #endregion
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderMasking> ChildContent { get; set; } = _ => _ => { };
    #endregion
    #region 占满全屏
    [Parameter]
    public bool IsFullScreen { get; set; }
    #endregion
    #region 蒙版模式
    /// <summary>
    /// 这个参数返回蒙版模式，
    /// 如果不指定，默认为亚克力蒙版
    /// </summary>
    [Parameter]
    public Func<IMaskingParameter, RenderMasking> MaskingMod { get; set; } = MaskingAcrylic;
    #endregion
    #endregion
    #region 静态成员
    #region 返回亚克力蒙版模式
    /// <summary>
    /// 返回亚克力蒙版模式
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="MaskingBase(IMaskingParameter, string, string)"/>
    public static RenderMasking MaskingAcrylic(IMaskingParameter parameter)
        => MaskingBase(parameter, "acrylicContainer", "acrylicMask");
    #endregion
    #region 返回苹果风格蒙版
    /// <summary>
    /// 返回一个类似苹果风格的蒙版模式
    /// </summary>
    /// <param name="isDarkness">如果这个值为<see langword="true"/>，
    /// 返回深色模式，否则返回浅色模式</param>
    /// <returns></returns>
    public static Func<IMaskingParameter, RenderMasking> MaskingIOS(bool isDarkness)
        => x => MaskingBase(x, $"iosContainer {(isDarkness ? "iosContainerDarkness" : "iosContainerUndertint")}");
    #endregion
    #region 基础方法
    /// <summary>
    /// 返回蒙版模式的基础方法
    /// </summary>
    /// <param name="parameter">创建蒙版模式的参数</param>
    /// <param name="externalCSS">外部容器CSS</param>
    /// <param name="internalCSS">内部容器CSS，
    /// 如果为<see langword="null"/>，表示不需要</param>
    /// <returns></returns>
    private static RenderMasking MaskingBase(IMaskingParameter parameter, string externalCSS, string? internalCSS = null)
    {
        var isFullScreen = parameter.IsFullScreen;
        var additionalCss = isFullScreen ? "occupyScre" : "";
        var fullScreenCss = isFullScreen ? "maskingContainerFullScreen" : "";
        return new()
        {
            ExternalCSS = $"{externalCSS} {additionalCss} {fullScreenCss}",
            InternalCSS = internalCSS is null ? null : $"{internalCSS} {additionalCss}"
        };
    }
    #endregion
    #endregion
}
