using System.Linq.Expressions;

using OpenQA.Selenium;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个类型是使用WebDriver实现的浏览器元素的基类
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="browser">封装的浏览器对象，本对象的功能就是通过它实现的</param>
abstract class ElementBrowserWebDriverBase(BrowserWebDriver browser) : IElementBrowser
{
    #region 公开成员
    #region 关于元素本身
    #region 读写浏览器属性
    private IAsyncIndex<string, string>? IndexField;

    public IAsyncIndex<string, string> Index
        => IndexField ??= CreateTasks.AsyncIndex<string, string>(
            (name, _) => Task.FromResult(Element.GetDomProperty(name) ?? Element.GetCssValue(name)),
            (_, _, _) => throw new NotImplementedException());
    #endregion
    #region 标签类型
    public string Type => Element.TagName;
    #endregion
    #region 元素的显示文本
    public string Text => Element.Text;
    #endregion
    #region 点击并创建一个新选项卡
    public async Task<IDisposable> ClickWithCreateTab()
    {
        await Click();
        Browser.Tabs[^1].Select();
        return FastRealize.Disposable(null, () => Browser.Tabs[^1].Dispose());
    }
    #endregion
    #endregion 
    #region 搜索子元素
    #region 指定CSS选择器
    public IReadOnlyList<Element> FindFromCss<Element>(string cssSelect, bool ignoreException)
        where Element : IElementBase
    {
        try
        {
            var elements = this.Element.FindElements(By.CssSelector(cssSelect)).
                Select(x => x.TagName switch
            {
                "input" => new ElementBrowserInputWebDriver(Browser, x),
                _ => new ElementBrowserWebDriver(Browser, x),
            }).OfType<Element>().ToArray();
            return elements;
        }
        catch (WebDriverTimeoutException) when (ignoreException)
        {
            return [];
        }
    }
    #endregion
    #region 指定表达式
    public IReadOnlyList<Element> Find<Element>(Expression<Func<Element, bool>> where, bool ignoreException)
        where Element : IElementBase
    {
        throw new NotImplementedException();
    }
    #endregion
    #endregion
    #region 关于元素交互
    #region 提交表单
    public ValueTask Submit(CancellationToken cancellationToken = default)
    {
        Element.Submit();
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 单击事件
    public async ValueTask Click(int interval = 0, CancellationToken cancellationToken = default)
    {
        Element.Click();
        if (interval > 0)
            await Task.Delay(interval, cancellationToken);
    }
    #endregion
    #endregion 
    #endregion
    #region 内部成员
    #region 浏览器
    /// <summary>
    /// 获取封装的浏览器对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected BrowserWebDriver Browser { get; } = browser;
    #endregion
    #region WebDriver对象
    /// <summary>
    /// 获取浏览器中的WebDriver对象
    /// </summary>
    protected WebDriver WebDriver => Browser.WebDriver;
    #endregion
    #region 浏览器元素
    /// <summary>
    /// 获取封装的浏览器元素，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected abstract IWebElement Element { get; }
    #endregion
    #endregion
    #region 未实现的成员
    public string? ID => throw new NotImplementedException();

    public string CssClass => throw new NotImplementedException();

    #endregion
}
