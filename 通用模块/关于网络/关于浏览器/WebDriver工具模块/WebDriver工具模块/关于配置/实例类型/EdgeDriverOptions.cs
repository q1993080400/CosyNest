using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个记录是Edge的浏览器选项
/// </summary>
public sealed record EdgeDriverOptions : WebDriverOptions
{
    #region 抽象类实现
    internal override EdgeOptions GenerateBrowserOptions()
    {
        var options = new EdgeOptions()
        {
            EnableDownloads = true,
            ImplicitWaitTimeout = TimeOut
        };
        if (Maximize)
            options.AddArgument("--start-maximized");
        if (IsHeadLess)
            options.AddArgument("--headless");
        if (Proxy is { } proxy)
        {
            if (proxy.Credentials is { })
                throw new NotSupportedException("Edge浏览器不支持带有身份验证的代理设置，请删除它");
            var host = proxy.Host;
            var driverProxy = new Proxy()
            {
                SocksProxy = host,
                SocksVersion = 5
            };
            options.Proxy = driverProxy;
        }
        return options;
    }
    #endregion
}
