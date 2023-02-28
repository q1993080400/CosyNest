using OpenQA.Selenium;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个类型是使用WebDriver实现的Body元素
/// </summary>
sealed class ElementBrowserWebDriverBody : ElementBrowserWebDriverBase
{
    #region 浏览器元素
    protected override IWebElement Element => WebDriver.FindElement(By.CssSelector("body"));
    #endregion 
    #region 构造函数
    /// <inheritdoc cref="ElementBrowserWebDriverBase(BrowserWebDriver)"/>
    public ElementBrowserWebDriverBody(BrowserWebDriver browser)
        : base(browser)
    {

    }
    #endregion
}
