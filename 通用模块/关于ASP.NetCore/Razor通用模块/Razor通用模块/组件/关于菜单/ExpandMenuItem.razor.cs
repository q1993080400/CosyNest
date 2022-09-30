namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件代表一个展开式菜单的项，
/// 当鼠标进入它的时候，会在右边或下边展开一个子级菜单
/// </summary>
public sealed partial class ExpandMenuItem : ComponentBase
{
    #region 说明文档
    /*本组件的原理：
     
      在初始化或刷新组件时：
      计算并缓存子菜单的位置
      设置子菜单的位置
    
      在指针划过菜单时：
      计算并缓存子菜单的位置
      设置子菜单的位置
      显示子菜单
    
      当指针离开子菜单时：
      计算并缓存子菜单的位置
      设置子菜单的位置
      隐藏子菜单
    
      本组件的原理比较复杂，这是因为web标准中一个关键API，
      getBoundingClientRect的设计缺陷，在某些情况下，
      即便父菜单已经显示，它仍然没办法获取父菜单的大小和范围
    
      以下情况建议不要使用本组件：
    
      #手机端Web应用，手机端不支持指针悬停，
      所以子菜单永远无法显示
    
      #会频繁改变位置的动态菜单，
      本组件可能无法正确计算子菜单的位置*/
    #endregion
    #region 组件参数
    #region 渲染菜单项的委托
    /// <summary>
    /// 这个委托用来渲染菜单项
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment MenuItem { get; set; }
    #endregion
    #region 渲染子级菜单的委托
    /// <summary>
    /// 这个委托用来渲染子级菜单，
    /// 它只在菜单展开的时候才会渲染
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment MenuSub { get; set; }
    #endregion
    #region 是否右侧弹出
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表菜单在右侧弹出，否则代表在下方弹出
    /// </summary>
    [Parameter]
    public bool IsPopRight { get; set; } = true;
    #endregion
    #region 是否次级菜单
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个菜单是一个次级菜单，否则表示是顶级菜单
    /// </summary>
    [CascadingParameter(Name = nameof(IsSub))]
    private bool IsSub { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 用于缓存元素矩形的属性名称
    /// <summary>
    /// 这个名称对应一个windows对象上的属性，
    /// 它可以用于缓存
    /// </summary>
    private string CacheRectangle { get; } = ToolASP.CreateJSObjectName();
    #endregion
    #region 菜单ID
    /// <summary>
    /// 获取菜单的ID
    /// </summary>
    private string MenuID { get; } = Guid.NewGuid().ToString();
    #endregion
    #region 子菜单ID
    /// <summary>
    /// 获取子菜单的ID
    /// </summary>
    private string SubID { get; } = Guid.NewGuid().ToString();
    #endregion
    #region 指针划过时触发的事件
    /// <summary>
    /// 指针划过时触发的事件脚本
    /// </summary>
    private string OnPointerOver => $$"""
if(Object.hasOwn(window,'{{CacheRectangle}}'))
{
var b={{IsPopRight.ToString().ToLower()}};
var e=document.getElementById('{{MenuID}}');
var r=window.{{CacheRectangle}};
var sub=document.getElementById('{{SubID}}');
sub.style.display='block';
sub.style.top=(b?r.top:r.bottom)+'px';
sub.style.left=(b?r.right:r.left)+'px';
}
""";
    #endregion
    #region 指针离开时触发的事件
    /// <summary>
    /// 指针离开时触发的事件脚本
    /// </summary>
    private string OnPointerOut => $$"""
var e=document.getElementById('{{MenuID}}');
window.{{CacheRectangle}}=e.getBoundingClientRect();
var sub=document.getElementById('{{SubID}}');
sub.style.display='none';
""";
    #endregion
    #region 重写OnAfterRenderAsync方法
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!IsSub)
            await Task.Delay(20);
        if (firstRender)
        {
            var script = OnPointerOut + "sub.style.opacity=1;";
            await JSRuntime.InvokeCodeVoidAsync(script);
        }
        else
        {
            var script = $$"""
                var b={{IsPopRight.ToString().ToLower()}};
                var e=document.getElementById('{{MenuID}}');
                var r=e.getBoundingClientRect();
                if(r.bottom!=0&&r.right!=0)
                {
                window.{{CacheRectangle}}=r;
                }
                var sub=document.getElementById('{{SubID}}');
                sub.style.top=(b?r.top:r.bottom)+'px';
                sub.style.left=(b?r.right:r.left)+'px';
                """;
            await JSRuntime.InvokeCodeVoidAsync(script);
        }
    }
    #endregion
    #endregion
}
