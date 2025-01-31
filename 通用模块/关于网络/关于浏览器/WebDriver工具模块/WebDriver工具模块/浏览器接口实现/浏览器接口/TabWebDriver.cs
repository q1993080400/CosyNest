using OpenQA.Selenium;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器标签
/// </summary>
/// <param name="browser">本标签所隶属的浏览器对象</param>
/// <param name="windowHandle">窗口句柄，它可以用来标识窗口</param>
sealed class TabWebDriver(BrowserWebDriver browser, string windowHandle) : ITab
{
    #region 公开成员
    #region 选择选项卡
    public void Select()
    {
        WebDriver.SwitchTo().Window(windowHandle);
    }
    #endregion
    #region 浏览器对象
    public IBrowser Browser => browser;
    #endregion
    #region 指示对象是否可用
    public bool IsAvailable
        => (Browser.IsAvailable, IsDispose) is (true, false);
    #endregion
    #region 释放对象
    public void Dispose()
    {
        if (!IsAvailable)
            return;
        IsDispose = true;
        try
        {
            WebDriver.SwitchTo().Window(windowHandle).Close();
        }
        catch (NoSuchWindowException)
        {
        }
    }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的浏览器对象
    /// <summary>
    /// 获取封装的浏览器对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private WebDriver WebDriver => browser.WebDriver;
    #endregion
    #region 指示对象是否被释放
    /// <summary>
    /// 指示这个对象是否被释放
    /// </summary>
    private bool IsDispose { get; set; }
    #endregion
    #endregion
}
