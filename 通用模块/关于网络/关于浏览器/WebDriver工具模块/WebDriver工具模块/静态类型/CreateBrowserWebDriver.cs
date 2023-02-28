using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个静态类可以用来创建使用WebDriver实现的浏览器
/// </summary>
public static class CreateBrowserWebDriver
{
    #region 创建任意浏览器
    /// <summary>
    /// 创建任意浏览器
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BrowserWebDriver(Func{WebDriver}, TimeSpan?)"/>
    public static IBrowser Browser(Func<WebDriver> factory, TimeSpan? timeOut = null)
        => new BrowserWebDriver(factory, timeOut);
    #endregion
    #region 创建Edge浏览器
    /// <summary>
    /// 创建Edge浏览器
    /// </summary>
    /// <param name="timeOut">指定每个操作的超时间隔，
    /// 如果为<see langword="null"/>，默认为3秒</param>
    /// <returns></returns>
    /// <inheritdoc cref="BrowserWebDriver(Func{WebDriver}, TimeSpan?)"/>
    public static IBrowser BrowserEdge(string uri, TimeSpan? timeOut = null)
        => new BrowserWebDriver(
            () =>
            {
                var driver = new EdgeDriver();
                driver.Navigate().GoToUrl(uri);
                return driver;
            }, timeOut);
    #endregion
}
