using System;
using System.Design.Async;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型是<see cref="IJSLocation"/>的实现，
    /// 可以视为一个JS中的Location对象
    /// </summary>
    class JSLocation : JSRuntimeBase, IJSLocation
    {
        #region 关于Uri
        #region 获取或设置当前Uri
        private IAsyncProperty<string>? HrefField;

        public IAsyncProperty<string> Href
            => HrefField ??= JSRuntime.PackProperty<string>("location.href");
        #endregion
        #region 获取主机名称
        public ValueTask<string> Host
            => JSRuntime.GetProperty<string>("location.host");
        #endregion
        #region 获取协议部分
        public ValueTask<string> Protocol
              => JSRuntime.GetProperty<string>("location.protocol");
        #endregion
        #endregion
        #region 刷新页面
        public ValueTask Reload(bool forceGet = false)
            => JSRuntime.InvokeVoidAsync("location.reload", forceGet);
        #endregion
        #region 构造函数
        /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
        public JSLocation(IJSRuntime jsRuntime)
            : base(jsRuntime)
        {

        }
        #endregion
    }
}
