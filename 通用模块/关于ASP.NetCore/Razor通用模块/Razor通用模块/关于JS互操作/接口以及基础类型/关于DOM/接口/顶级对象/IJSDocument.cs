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
    #region 返回页面的可见状态
    /// <summary>
    /// 返回页面的可见状态
    /// </summary>
    Task<VisibilityState> VisibilityState { get; }
    #endregion
}
