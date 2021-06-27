
using System.Collections.Generic;
using System.Design.Async;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为JS中的Document对象的Net封装
    /// </summary>
    public interface IJSDocument
    {
        #region 返回Cookie对象
        /// <summary>
        /// 返回一个字典，它可以用来索引Cookie
        /// </summary>
        IAsyncDictionary<string, string> Cookie { get; }
        #endregion
        #region 获取或设置标题
        /// <summary>
        /// 通过这个异步属性，
        /// 可以获取或设置窗口的标题
        /// </summary>
        IAsyncProperty<string> Title { get; }
        #endregion
    }
}
