namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可以用来实现水印
/// </summary>
public sealed partial class Watermark : ComponentBase, IContentComponent<RenderFragment<string>>
{
    #region 组件参数
    #region 用于渲染水印的委托
    /// <summary>
    /// 获取用来渲染水印的委托，
    /// 它的内部只能包含一个svg标签，
    /// 这个委托的参数是一个ID，必须将它赋值给那个svg标签
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> ChildContent { get; set; }
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
    #region 获取SVG的ID
    /// <summary>
    /// 获取水印SVG的ID
    /// </summary>
    private string SVGID { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region SVG图片的Uri
    /// <summary>
    /// 获取经过Blob编码后的SVGUri
    /// </summary>
    private string? SVGUri { get; set; }
    #endregion
    #region 需要渲染Uri
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 指示组件参数已经改变，需要重新渲染SVG的Uri
    /// </summary>
    private bool NeedRenderUri { get; set; }
    #endregion
    #region 重写OnParametersSet方法
    protected override void OnParametersSet()
    {
        NeedRenderUri = true;
    }
    #endregion
    #region 重写OnAfterRenderAsync方法
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (NeedRenderUri)
        {
            NeedRenderUri = false;
            SVGUri = await JSWindow.InvokeAsync<string>("CreateSVGUri", new[] { SVGID });
            this.StateHasChanged();
        }
    }
    #endregion
    #endregion
}
