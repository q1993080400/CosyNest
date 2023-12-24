using OpenQA.Selenium;

using System.Design;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器标签
/// </summary>
/// <remarks>
/// 使用指定的Edge测试用例初始化对象
/// </remarks>
/// <param name="browser">本标签所隶属的浏览器对象</param>
/// <param name="windowHandle">窗口句柄，它可以用来标识窗口</param>
sealed class TabWebDriver(BrowserWebDriver browser, string windowHandle) : Release, ITab
{
    #region 公开成员
    #region 选择选项卡
    public void Select()
    {
        WebDriver.SwitchTo().Window(WindowHandle);
        Browser.CurrentTab = this;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 浏览器对象
    IBrowser ITab.Browser => Browser;

    /// <summary>
    /// 本标签所隶属的浏览器对象
    /// </summary>
    internal BrowserWebDriver Browser { get; } = browser;
    #endregion

    /// <summary>
    /// 获取封装的浏览器对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private WebDriver WebDriver => Browser.WebDriver;
    #region 释放对象
    protected override void DisposeRealize()
    {
        Select();
        WebDriver.Close();
        var tabs = Browser.Tabs;
        tabs.Remove(this);
        if (tabs.Count != 0)
            tabs[^1].Select();
    }
    #endregion
    #region 窗口句柄
    /// <summary>
    /// 获取窗口句柄，它可以用来标识窗口
    /// </summary>
    private string WindowHandle { get; } = windowHandle;

    #endregion
    #endregion
    #region 构造函数
    #endregion
}
