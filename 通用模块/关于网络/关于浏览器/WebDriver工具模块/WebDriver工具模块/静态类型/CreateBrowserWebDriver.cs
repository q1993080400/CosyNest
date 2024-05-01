using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个静态类可以用来创建使用WebDriver实现的浏览器
/// </summary>
public static class CreateBrowserWebDriver
{
    #region 公开成员
    #region 创建Edge浏览器
    /// <summary>
    /// 创建Edge浏览器
    /// </summary>
    /// <param name="uri">浏览器打开时显示的Uri，
    /// 如果为<see langword="null"/>，则不执行这个操作</param>
    /// <returns></returns>
    /// <inheritdoc cref="BrowserWebDriver(WebDriver, WebDriverOptions?)"/>
    public static IBrowser BrowserEdge(string? uri = null, WebDriverOptions? options = null)
        => Browser<EdgeDriver, EdgeOptions>(uri, options, (options, driverOptions) =>
        {
            if (options.Maximize)
                driverOptions.AddArgument("--start-maximized");
            if (options is { Proxy.Credentials: { } })
                throw new NotSupportedException("Edge浏览器不支持带有身份验证的代理设置，请删除它");
            return new(driverOptions);
        });
    #endregion
    #region 创建火狐浏览器
    public static IBrowser BrowserFirefox(string? uri = null, WebDriverOptions? options = null)
        => Browser<FirefoxDriver, FirefoxOptions>(uri, options, (_, x) => new(x));
    #endregion
    #endregion
    #region 内部成员
    #region 创建任何浏览器
    /// <summary>
    /// 创建任意类型的浏览器
    /// </summary>
    /// <typeparam name="Browser">浏览器类型</typeparam>
    /// <typeparam name="Options">浏览器选项类型</typeparam>
    /// <param name="createBrowser">传入上层和下层配置，创建浏览器的委托</param>
    /// <returns></returns>
    /// <inheritdoc cref="BrowserWebDriver(WebDriver, WebDriverOptions?)"/>
    private static BrowserWebDriver Browser<Browser, Options>
        (string? uri, WebDriverOptions? options, Func<WebDriverOptions, Options, Browser> createBrowser)
        where Browser : WebDriver
        where Options : DriverOptions, new()
    {
        options ??= new();
        var browserOptions = CreateOptions<Options>(options);
        var driver = createBrowser(options, browserOptions);
        if (uri is { })
            driver.Navigate().GoToUrl(uri);
        return new BrowserWebDriver(driver, options);
    }
    #endregion
    #region 创建浏览器配置
    /// <summary>
    /// 创建浏览器配置
    /// </summary>
    /// <typeparam name="Options">浏览器配置的类型</typeparam>
    /// <param name="options">上层浏览器配置</param>
    /// <returns></returns>
    private static Options CreateOptions<Options>(WebDriverOptions options)
        where Options : DriverOptions, new()
    {
        var driverOptions = new Options()
        {
            EnableDownloads = true,
        };
        AddProxy(driverOptions, options);
        return driverOptions;
    }
    #endregion
    #region 为浏览器添加代理
    /// <summary>
    /// 为浏览器添加代理
    /// </summary>
    /// <param name="driverOptions">底层浏览器配置选项</param>
    /// <param name="webDriverOptions">上层浏览器配置选项</param>
    private static void AddProxy(DriverOptions driverOptions, WebDriverOptions webDriverOptions)
    {
        if (webDriverOptions.Proxy is not { } proxy)
            return;
        var host = proxy.Host;
        var driverProxy = new Proxy()
        {
            SocksProxy = host,
        };
        if (proxy.Credentials is { } credentials)
        {
            driverProxy.SocksUserName = credentials.ID;
            driverProxy.SocksPassword = credentials.Password;
        }
        driverProxy.SocksVersion = 5;
        driverOptions.Proxy = driverProxy;
    }
    #endregion
    #endregion
}
