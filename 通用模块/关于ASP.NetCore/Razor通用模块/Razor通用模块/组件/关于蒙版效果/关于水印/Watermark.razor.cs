namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以用来实现水印
/// </summary>
public sealed partial class Watermark : ComponentBase, IContentComponent<RenderFragment>
{
    #region 组件参数
    #region 用于渲染水印的委托
    /// <summary>
    /// 获取用来渲染水印的委托，
    /// 它的内部只能包含一个svg标签
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 背景图片的大小
    /// <summary>
    /// 获取背景图片的大小，
    /// 它的语法和background-size相同
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string ImageSize { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取SVG容器的ID
    /// <summary>
    /// 获取水印SVG容器的ID
    /// </summary>
    private string SVGContainerID { get; } = CreateASP.JSObjectName();
    #endregion
    #region SVG图片的Uri
    /// <summary>
    /// 获取经过Blob编码后的SVGUri
    /// </summary>
    private string? SVGUri { get; set; }
    #endregion
    #region 重写OnParametersSetAsync方法
    protected override async Task OnParametersSetAsync()
    {
        SVGUri = await JSWindow.InvokeAsync<string>("CreateSVGUri", new[] { SVGContainerID });
    }
    #endregion
    #endregion
}
