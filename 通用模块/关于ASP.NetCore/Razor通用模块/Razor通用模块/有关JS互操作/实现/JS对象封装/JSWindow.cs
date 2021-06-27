using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型是<see cref="IJSWindow"/>的实现，
    /// 可以视为一个Window对象
    /// </summary>
    class JSWindow : JSRuntimeBase, IJSWindow
    {
        #region JS对象
        #region 返回Document对象
        private IJSDocument? DocumentField;

        public IJSDocument Document
            => DocumentField ??= new JSDocument(JSRuntime);
        #endregion
        #region 返回本地存储对象
        private JSLocalStorage? LocalStorageField;

        public IAsyncDictionary<string, string> LocalStorage
             => LocalStorageField ??= new(JSRuntime);
        #endregion
        #region 返回Location对象
        private JSLocation? LocationField;

        public IJSLocation Location
            => LocationField ??= new(JSRuntime);
        #endregion
        #endregion
        #region JS方法
        #region 弹出确认窗
        public ValueTask<bool> Confirm(string message)
              => JSRuntime.InvokeAsync<bool>("confirm", message);
        #endregion
        #region 弹出消息窗
        public ValueTask Alert(string message)
               => JSRuntime.InvokeVoidAsync("alert", message);
        #endregion
        #region 打印窗口
        public ValueTask Print()
              => JSRuntime.InvokeVoidAsync("print");
        #endregion
        #region 关闭窗口
        public ValueTask Close()
             => JSRuntime.InvokeVoidAsync("close");
        #endregion
        #endregion
        #region 构造函数
        /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
        public JSWindow(IJSRuntime jsRuntime)
            : base(jsRuntime)
        {

        }
        #endregion
    }
}
