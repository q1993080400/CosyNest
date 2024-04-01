using OpenQA.Selenium;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个类型是使用WebDriver实现的Input元素
/// </summary>
/// <inheritdoc cref="ElementBrowserWebDriver(BrowserWebDriver, IWebElement)"/>
sealed class ElementBrowserInputWebDriver(BrowserWebDriver browser, IWebElement element) :
    ElementBrowserWebDriver(browser, element), IElementBrowserInput
{
    #region 输入字符串
    public ValueTask Input(string input, CancellationToken cancellationToken = default)
    {
        Element.SendKeys(input);
        return ValueTask.CompletedTask;
    }
    #endregion
}
