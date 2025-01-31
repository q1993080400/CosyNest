using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为JS中的Document对象的Net封装，
/// 它可以用来检查和修改DOM
/// </summary>
public interface IJSDocument
{
    #region 返回Cookie对象
    /// <summary>
    /// 返回一个字典，它可以用来索引Cookie
    /// </summary>
    ICookie Cookie { get; }

    /*说明文档
      设置Cookie时需要特别小心，它必须遵循以下原则，
      否则无法被服务器上的HttpRequest.Cookies所读取：
      #Cookie的值必须遵循ASCII编码，但是实际允许的值范围似乎更小，
      因为作者观察到，!无法被识别
      奇怪的是，浏览器本身支持Unicode编码的Cookie，
      无法理解ASP为什么会这么设计*/
    #endregion
    #region 获取或设置标题
    /// <summary>
    /// 通过这个异步属性，
    /// 可以获取或设置窗口的标题
    /// </summary>
    IAsyncProperty<string> Title { get; }
    #endregion
    #region 向任意元素注册事件
    /// <summary>
    /// 向任意HTML元素注册JS事件
    /// </summary>
    /// <param name="elementSelector">用来选择这个JS元素的CSS选择器，
    /// 它必须保证选择且只选择一个元素</param>
    /// <param name="eventName">要注册的JS事件的名称</param>
    /// <param name="func">用来执行事件的函数</param>
    /// <returns>一个可以用来释放事件的对象</returns>
    Task<IAsyncDisposable> RegisterEvent(string elementSelector, string eventName, Func<Task> func);
    #endregion
    #region 注册VisibilityChange事件
    /// <summary>
    /// 注册document.visibilityChange事件，
    /// 它在页面可见性改变的时候，会触发一个通知
    /// </summary>
    /// <param name="onVisibilityChange">要注册的事件委托</param>
    /// <returns>一个可以用来释放事件的对象</returns>
    Task<IAsyncDisposable> RegisterVisibilityChange(Func<VisibilityState, Task> onVisibilityChange);
    #endregion
    #region 返回页面的可见状态
    /// <summary>
    /// 返回页面的可见状态
    /// </summary>
    Task<VisibilityState> VisibilityState { get; }
    #endregion
}
