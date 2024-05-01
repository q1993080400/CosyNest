using OpenQA.Selenium;

using System.Design;
using System.Underlying.PC;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器
/// </summary>
sealed class BrowserWebDriver : AutoRelease, IBrowser
{
    #region 公开成员
    #region 有关选项卡
    #region 创建浏览器标签
    public ITab CreateTab(string uri)
    {
        WebDriver.SwitchTo().NewWindow(WindowType.Tab);
        var tab = new TabWebDriver(this, WebDriver.CurrentWindowHandle);
        tab.Select();
        Uri = uri;
        return tab;
    }
    #endregion
    #region 选项卡的集合
    public IReadOnlyList<ITab> Tabs
        => WebDriver.WindowHandles.
        Select(x => new TabWebDriver(this, x)).ToArray();
    #endregion
    #region 当前选项卡
    public ITab CurrentTab
        => new TabWebDriver(this, WebDriver.CurrentWindowHandle);
    #endregion
    #endregion
    #region 有关浏览器内容
    #region 获取根节点元素
    public IElementBrowser Body { get; }
    #endregion
    #region 选项卡Uri
    public string Uri
    {
        get => WebDriver.Url;
        set => WebDriver.Navigate().GoToUrl(value);
    }
    #endregion
    #region 键盘模拟器
    public IKeyBoard KeyboardEmulation
        => new KeyboardWebDriver(WebDriver);
    #endregion
    #region 关于执行脚本
    #region 无返回值
    public ValueTask InvokingScriptVoid(string script, CancellationToken cancellationToken = default)
    {
        WebDriver.ExecuteScript(script);
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 有返回值
    public ValueTask<Obj> InvokingScript<Obj>(string script, CancellationToken cancellationToken = default)
    {
        var obj = WebDriver.ExecuteScript(script);
        return ValueTask.FromResult(obj.To<Obj>());
    }
    #endregion
    #endregion 
    #endregion
    #endregion
    #region 内部成员
    #region 浏览器实例
    /// <summary>
    /// 获取浏览器的实例，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal WebDriver WebDriver { get; }
    #endregion
    #region 浏览器选项
    /// <summary>
    /// 获取浏览器的配置选项
    /// </summary>
    internal WebDriverOptions Options { get; }
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
    {
        WebDriver.Quit();
        WebDriver.Dispose();
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="webDriver">浏览器实例</param>
    /// <param name="options">用来创建浏览器的选项，
    /// 如果为<see langword="null"/>，则使用默认选项</param>
    public BrowserWebDriver(WebDriver webDriver, WebDriverOptions options)
    {
        WebDriver = webDriver;
        Options = options;
        Body = new ElementBrowserWebDriverBody(this);
    }
    #endregion
}
