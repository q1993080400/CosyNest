using OpenQA.Selenium;

using System.Design;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器
/// </summary>
sealed class BrowserWebDriver : AutoRelease, IBrowser
{
    #region 公开成员
    #region 创建浏览器标签
    public ITab CreateTab(string uri)
    {
        var driver = Factory();
        var tab = new TabWebDriver(driver, this)
        {
            Uri = uri
        };
        Tabs.Add(tab);
        return tab;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 创建浏览器实例的工厂
    /// <summary>
    /// 获取用来创建浏览器实例的工厂
    /// </summary>
    private Func<WebDriver> Factory { get; }
    #endregion
    #region 等待超时
    /// <summary>
    /// 获取每个操作等待超时的时间限制
    /// </summary>
    internal TimeSpan TimeOut { get; }
    #endregion
    #region 缓存标签的集合
    internal LinkedList<ITab> Tabs { get; } = new();
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
    {
        Tabs.ToArray().ForEach(x => x.Dispose());
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="factory">用来创建浏览器实例的工厂</param>
    /// <param name="timeOut">每个操作等待超时的时间限制</param>
    public BrowserWebDriver(Func<WebDriver> factory, TimeSpan timeOut)
    {
        Factory = factory;
        TimeOut = timeOut;
    }
    #endregion
}
