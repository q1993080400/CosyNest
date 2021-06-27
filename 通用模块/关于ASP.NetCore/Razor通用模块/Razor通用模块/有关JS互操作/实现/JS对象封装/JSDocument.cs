
using System;
using System.Collections.Generic;
using System.Design.Async;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型是<see cref="IJSDocument"/>的实现，
    /// 可以视为一个JS中的Document对象
    /// </summary>
    class JSDocument : JSRuntimeBase, IJSDocument
    {
        #region 返回索引Cookie的字典
        private JSCookie? CookieField;

        public IAsyncDictionary<string, string> Cookie
            => CookieField ??= new(JSRuntime);
        #endregion
        #region 获取或设置标题
        private IAsyncProperty<string>? TitleField;

        public IAsyncProperty<string> Title
            => TitleField ??= JSRuntime.PackProperty<string>("document.title");
        #endregion
        #region 构造函数
        /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
        public JSDocument(IJSRuntime jsRuntime)
            : base(jsRuntime)
        {

        }
        #endregion
    }
}
