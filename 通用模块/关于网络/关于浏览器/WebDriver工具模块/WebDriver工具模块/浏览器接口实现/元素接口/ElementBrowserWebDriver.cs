using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System.Linq.Expressions;

namespace System.NetFrancis.Browser;

/// <summary>
/// 本类型是底层使用Selenium实现的浏览器元素
/// </summary>
sealed class ElementBrowserWebDriver : IElementBrowser
{
    #region 公开成员
    #region 获取浏览器标签
    ITab IElementBrowser.Tab => Tab;
    #endregion
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
    #region 搜索子元素
    #region 指定CSS选择器
    public IEnumerable<Element> FindFromCss<Element>(string cssSelect)
        where Element : IElementBase
    {
        var wait = new WebDriverWait(Tab.WebDriver, Browser.TimeOut);
        var elements = wait.Until(x =>
        {
            var e = this.Element.FindElements(By.CssSelector(cssSelect));
            return e.Any() ? e : null;
        });
        return elements!.Select(x => new ElementBrowserWebDriver(Tab, x)).Cast<Element>().ToArray();
    }
    #endregion
    #region 指定表达式
    public IEnumerable<Element> Find<Element>(Expression<Func<Element, bool>> where)
        where Element : IElementBase
    {
        throw new NotImplementedException();
    }
    #endregion
    #endregion
    #region 输入字符串
    public void Input(string input)
        => Element.SendKeys(input);
    #endregion
    #region 提交表单
    public ValueTask Submit(CancellationToken cancellationToken = default)
    {
        Element.Submit();
        return ValueTask.CompletedTask;
    }
    #endregion
    #region 单击事件
    public ValueTask Click(CancellationToken cancellationToken = default)
    {
        Element.Click();
        return ValueTask.CompletedTask;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 浏览器
    /// <summary>
    /// 获取封装的浏览器对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private BrowserWebDriver Browser
        => Tab.Browser;
    #endregion
    #region 浏览器标签（具体类型）
    /// <summary>
    /// 获取浏览器标签
    /// </summary>
    private TabWebDriver Tab { get; }
    #endregion
    #region 浏览器元素
    /// <summary>
    /// 获取封装的浏览器元素，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IWebElement Element { get; }
    #endregion
    #endregion
    #region 未实现的成员

    public string? ID => throw new NotImplementedException();


    public string CssClass => throw new NotImplementedException();


    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="tab">元素所在的浏览器页签</param>
    /// <param name="element">封装的浏览器元素，
    /// 本对象的功能就是通过它实现的</param>
    public ElementBrowserWebDriver(TabWebDriver tab, IWebElement element)
    {
        Element = element;
        Tab = tab;
    }
    #endregion
}
