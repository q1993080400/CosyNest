using OpenQA.Selenium;

using System.Design;
using System.Underlying.PC;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器标签
/// </summary>
sealed class TabWebDriver : Release, ITab
{
    #region 公开成员
    #region 获取根节点元素
    public IElementBrowser Body
    {
        get
        {
            var element = WebDriver.FindElement(By.CssSelector("body"));
            return new ElementBrowserWebDriver(this, element);
        }
    }
    #endregion
    #region 获取浏览器
    IBrowser ITab.Browser => Browser;
    #endregion
    #region 选项卡Uri
    public string Uri
    {
        get => WebDriver.Url;
        set => WebDriver.Url = value;
    }
    #endregion
    #region 键盘模拟器
    public IKeyBoard KeyboardEmulation
        => new KeyboardWebDriver(WebDriver);
    #endregion
    #endregion
    #region 内部成员
    #region 浏览器测试用例
    /// <summary>
    /// 测试用例，通过它可以控制浏览器
    /// </summary>
    internal WebDriver WebDriver { get; }
    #endregion
    #region 浏览器对象
    /// <summary>
    /// 本标签所隶属的浏览器对象
    /// </summary>
    internal BrowserWebDriver Browser { get; }
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
    {
        WebDriver.Quit();
        Browser.Tabs.Remove(this);
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的Edge测试用例初始化对象
    /// </summary>
    /// <param name="webDriver">Edge测试用例，通过它可以控制浏览器</param>
    /// <param name="browser">本标签所隶属的浏览器对象</param>
    public TabWebDriver(WebDriver webDriver, BrowserWebDriver browser)
    {
        WebDriver = webDriver;
        Browser = browser;
    }
    #endregion
}
