namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是使用VideoJS实现的播放器
/// </summary>
public sealed partial class VideoJS : ComponentBase
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderVideoJS RenderVideoJS { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;
        var options = new
        {
            Autoplay = RenderVideoJS.Autoplay ? "any" : (object)false,
            RenderVideoJS.Src,
            RenderVideoJS.Loop,
            RenderVideoJS.Controls,
            RenderVideoJS.Width,
            RenderVideoJS.Height,
            Responsive = true,
        };
        await JSRuntime.InvokeVoidAsync("InitializationVideoJS", RenderVideoJS.ID, options);
    }
    #endregion
    #endregion
}
