using System.IOFrancis.FileSystem;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件可用于预览图片大图或视频，
/// 并支持滑动
/// </summary>
public sealed partial class MultimediaPreview : ComponentBase
{
    #region 组件参数
    #region 要渲染的媒体Uri
    /// <summary>
    /// 获取要渲染的视频或图片的Uri
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<string> MediumUri { get; set; }
    #endregion
    #region 渲染视频的委托
    /// <summary>
    /// 获取用来渲染视频的委托，
    /// 委托参数就是视频的Uri
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> RenderVideo { get; set; }
    #endregion
    #region 渲染图片的委托
    /// <summary>
    /// 获取用来渲染图片的委托，
    /// 委托参数就是图片的Uri
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<string> RenderImage { get; set; }
    #endregion
    #region 渲染外壳的委托
    /// <summary>
    /// 获取用来渲染外壳的委托，它包裹图片或视频，
    /// 参数就是用来渲染图片或视频的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFragment> RenderHull { get; set; }
    #endregion
    #region 渲染组件的委托
    /// <summary>
    /// 获取用来渲染整个组件的委托，
    /// 委托参数就是渲染每个图片或视频的方法
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<IEnumerable<RenderFragment>> RenderComponent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 枚举渲染参数
    /// <summary>
    /// 枚举所有用来渲染这个组件的参数
    /// </summary>
    /// <returns></returns>
    private IEnumerable<RenderFragment> GetRenderInfos()
    {
        var video = FileTypeCom.WebVideo;
        var image = FileTypeCom.WebImage;
        foreach (var item in MediumUri)
        {
            switch (item)
            {
                case var uri when image.IsCompatible(uri):
                    yield return RenderHull(RenderImage(uri));
                    break;
                case var uri when video.IsCompatible(uri):
                    yield return RenderHull(RenderVideo(uri));
                    break;
                case var uri:
                    throw new ArgumentException($"{uri}既不是一个受支持的图片Uri，也不是一个受支持的视频Uri");
            }
        }
    }
    #endregion
    #endregion
}
