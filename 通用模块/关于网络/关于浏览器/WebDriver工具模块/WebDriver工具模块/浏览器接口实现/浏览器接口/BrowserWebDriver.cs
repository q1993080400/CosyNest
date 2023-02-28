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
    IEnumerable<ITab> IBrowser.Tabs => Tabs;

    /// <summary>
    /// 所有选项卡的集合
    /// </summary>
    internal List<ITab> Tabs { get; } = new();
    #endregion
    #region 当前选项卡
    public ITab CurrentTab { get; set; }
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
    #region 执行脚本
    public ValueTask InvokingScript(string script, CancellationToken cancellationToken = default)
    {
        WebDriver.ExecuteScript(script);
        return ValueTask.CompletedTask;
    }
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
    #region 等待超时
    /// <summary>
    /// 获取每个操作等待超时的时间限制
    /// </summary>
    internal TimeSpan TimeOut { get; }
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
    /// <param name="factory">用来创建浏览器实例的工厂</param>
    /// <param name="timeOut">每个操作等待超时的时间限制，
    /// 如果为<see langword="null"/>，默认为3秒</param>
    public BrowserWebDriver(Func<WebDriver> factory, TimeSpan? timeOut)
    {
        WebDriver = factory();
        TimeOut = timeOut ?? TimeSpan.FromSeconds(3);
        var tab = new TabWebDriver(this, WebDriver.CurrentWindowHandle);
        Tabs.Add(tab);
        CurrentTab = tab;
        Body = new ElementBrowserWebDriverBody(this);
    }
    #endregion
}
