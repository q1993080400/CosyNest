using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace Microsoft.AspNetCore.Components
{
    /// <summary>
    /// 这个组件不显示任何UI，
    /// 而是用于修改HTML文档的Head部分
    /// </summary>
    public class HeadModify : ComponentBase
    {
#pragma warning disable CS8618

        #region 依赖注入的JS运行时
        /// <summary>
        /// 依赖注入的JS运行时
        /// </summary>
        [Inject]
        private IJSWindow JSWindow { get; set; }
        #endregion
        #region 可修改的Head部分属性
        #region 标题
        /// <summary>
        /// 修改文档的标题，
        /// 如果为<see langword="null"/>，代表不修改
        /// </summary>
        [Parameter]
        public string? Title { get; set; }
        #endregion
        #endregion
        #region 重写OnAfterRenderAsync
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (Title is { })
                await JSWindow.Document.Title.Set(Title);
        }
        #endregion
    }
}
