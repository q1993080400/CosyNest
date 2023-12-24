using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件是<see cref="FileViewer"/>的开箱即用版，
/// 它支持通过九宫格（或更多）的方式预览媒体或文件，
/// 并支持滑动等功能
/// </summary>
public sealed partial class BootstrapFileViewer : ComponentBase
{
    #region 组件参数
    #region 要展示的媒体或文件
    /// <summary>
    /// 获取或设置要展示的媒体或文件
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<FileSource> FileSource { get; set; }
    #endregion
    #region 用来渲染组件的委托
    /// <summary>
    /// 获取用来渲染组件主体部分的委托，
    /// 它的参数是一个集合，集合的元素是渲染每一个封面的委托，
    /// 如果为<see langword="null"/>，则使用一个默认方法
    /// </summary>
    [Parameter]
    public RenderFragment<IEnumerable<RenderFragment>>? RenderComponent { get; set; }
    #endregion
    #region 渲染封面的委托
    /// <summary>
    /// 获取或设置渲染封面的委托，
    /// 如果为<see langword="null"/>，
    /// 则使用一个默认方法
    /// </summary>
    [Parameter]
    public RenderFragment<BootstrapRenderCoverInfo>? RenderCover { get; set; }
    #endregion
    #region 封面大小
    /// <summary>
    /// 获取描述封面大小的样式（不是CSS类名），
    /// 封面是一个正方形
    /// </summary>
    [Parameter]
    public string? CoverSize { get; set; }

    /*说明文档
      对于指定这个属性的原则是：
      如果父容器的宽度是确定的，则使用容器查询
      如果父容器的宽度不确定，则显式指定子容器宽度*/
    #endregion
    #endregion
    #region 内部成员
    #region 是否打开工具栏
    /// <summary>
    /// 获取是否打开工具栏
    /// </summary>
    private bool IsOpen { get; set; }
    #endregion
    #region 获取要渲染的媒体或文件
    /// <summary>
    /// 枚举要渲染的媒体或文件，
    /// 它以正在渲染的那个媒体作为起始顺序
    /// </summary>
    /// <param name="info">用来渲染组件的参数</param>
    /// <returns></returns>
    private static FileSource[] Files(RenderFileViewerInfo info)
    {
        var index = info.PreviewFileIndex;
        if (index < 0)
            return [];
        var medias = info.Files.ToArray();
        return medias[index..].Concat(medias[..index]).ToArray();
    }
    #endregion
    #region 转换封面渲染参数
    /// <summary>
    /// 转换封面渲染参数，使其能够支持本组件所需要的一些功能
    /// </summary>
    /// <param name="info">待转换的封面渲染参数</param>
    /// <returns></returns>
    private BootstrapRenderCoverInfo ConvertRenderCoverInfo(RenderCoverInfo info)
        => new()
        {
            RenderCoverInfo = info with
            {
                PreviewEvent = new(this, async () =>
                {
                    IsOpen = true;
                    await info.PreviewEvent.InvokeAsync();
                })
            },
            CoverSize = CoverSize
        };
    #endregion
    #endregion
}
