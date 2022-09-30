using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

using System.Underlying.PC;

using Keys = System.Underlying.PC.Keys;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个类型是<see cref="IKeyBoard"/>的实现，
/// 可以用来模拟浏览器键盘操作
/// </summary>
sealed class KeyboardWebDriver : IKeyBoard
{
    #region 公开成员
    #region 按下按键
    public void Down(params Keys[] keys)
    {
        var files = typeof(OpenQA.Selenium.Keys).GetTypeData().FieldDictionary;
        var keysSelenium = keys.Select(key =>
        {
            var k = key.ToString();
            var (exist, value) = files.TryGetValue(k);
            return exist ? value.Single().GetValue<string>(null)! : k;
        }).ToArray();
        var action = new Actions(WebDriver);
        if (keys[0] is Keys.ControlKey)
        {
            foreach (var item in keysSelenium)
            {
                action.KeyDown(item);
            }
            foreach (var item in keysSelenium)
            {
                action.KeyUp(item);
            }
        }
        else
        {
            foreach (var item in keysSelenium)
            {
                action.KeyDown(item);
                action.KeyUp(item);
            }
        }
        action.Perform();
    }
    #endregion
    #endregion
    #region 私有成员
    #region 浏览器测试用例
    /// <summary>
    /// 获取浏览器测试用例，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private WebDriver WebDriver { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="webDriver">浏览器测试用例，
    /// 本对象的功能就是通过它实现的</param>
    public KeyboardWebDriver(WebDriver webDriver)
    {
        WebDriver = webDriver;
    }
    #endregion
}
