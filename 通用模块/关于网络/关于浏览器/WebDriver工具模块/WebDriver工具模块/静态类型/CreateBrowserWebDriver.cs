using OpenQA.Selenium.Edge;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个静态类可以用来创建使用WebDriver实现的浏览器
/// </summary>
public static class CreateBrowserWebDriver
{
    #region 创建Edge浏览器
    /// <summary>
    /// 创建Edge浏览器
    /// </summary>
    /// <param name="options">浏览器的配置选项，
    /// 如果为<see langword="null"/>，表示使用默认选项</param>
    /// <returns></returns>
    public static IBrowser BrowserEdge(EdgeDriverOptions? options = null)
    {
        options ??= new();
        var browserOptions = options.GenerateBrowserOptions();
        var driver = new EdgeDriver(browserOptions);
        return new BrowserWebDriver(driver);
    }
    #endregion
}
