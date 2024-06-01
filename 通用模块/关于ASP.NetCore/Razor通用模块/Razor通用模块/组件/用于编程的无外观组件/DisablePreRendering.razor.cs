namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件允许禁用预呈现
/// </summary>
public sealed partial class DisablePreRendering : ComponentBase, IContentComponent<RenderFragment>
{
    #region 说明文档
    /*问：什么是预呈现？
      答：请自行查阅Blazor文档
    
      问：禁用预呈现有什么好处？
      答：它可以保证在任何生命周期阶段，都可以进行JS互操作，
      因为这个组件是在浏览器中出生的，它没有服务器预渲染阶段

      但是，它也会导致搜索引擎不容易抓取到它的内容
      
      问：Blazor原生就具有禁用预呈现的办法，
      那么，为什么还需要这个组件？
      答：因为原生的做法不够灵活，它是在指定渲染模式的时候，
      同时指定是否进行预呈现，但是指定渲染模式有很多限制*/
    #endregion
    #region 组件参数
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 是否应该渲染
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该渲染这个组件，否则不渲染组件
    /// </summary>
    private bool ShouldRendered { get; set; }
    #endregion
    #region 重写OnAfterRender
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            ShouldRendered = true;
            StateHasChanged();
        }
    }
    #endregion
    #endregion
}
