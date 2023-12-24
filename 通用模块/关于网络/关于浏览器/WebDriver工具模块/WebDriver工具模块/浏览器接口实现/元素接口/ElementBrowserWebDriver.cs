using OpenQA.Selenium;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器元素
/// </summary>
/// <param name="element">指定的浏览器元素</param>
/// <inheritdoc cref="ElementBrowserWebDriverBase(BrowserWebDriver)"/>
sealed class ElementBrowserWebDriver(BrowserWebDriver browser, IWebElement element) : ElementBrowserWebDriverBase(browser)
{
    #region 浏览器元素
    protected override IWebElement Element { get; } = element;
    #endregion
}
