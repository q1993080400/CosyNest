using OpenQA.Selenium;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器元素
/// </summary>
sealed class ElementBrowserWebDriver : ElementBrowserWebDriverBase
{
    #region 浏览器元素
    protected override IWebElement Element { get; }
    #endregion
    #region 构造函数
    /// <param name="element">指定的浏览器元素</param>
    /// <inheritdoc cref="ElementBrowserWebDriverBase(BrowserWebDriver)"/>
    public ElementBrowserWebDriver(BrowserWebDriver browser, IWebElement element)
        : base(browser)
    {
        Element = element;
    }
    #endregion
}
