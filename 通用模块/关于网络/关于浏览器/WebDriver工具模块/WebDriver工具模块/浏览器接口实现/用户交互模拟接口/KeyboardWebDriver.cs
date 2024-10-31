using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

using System.Reflection;
using System.Underlying.PC;

using Keys = System.Underlying.PC.Keys;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个类型是<see cref="IKeyBoard"/>的实现，
/// 可以用来模拟浏览器键盘操作
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="webDriver">浏览器测试用例，
/// 本对象的功能就是通过它实现的</param>
sealed class KeyboardWebDriver(WebDriver webDriver) : IKeyBoard
{
    #region 公开成员
    #region 按下按键
    public void Down(params Keys[] keys)
    {
        var files = typeof(OpenQA.Selenium.Keys).GetFields(CreateReflection.BindingFlagsAll).
            ToDictionary(x => x.Name, x => x);
        var keysSelenium = keys.Select(key =>
        {
            var k = key.ToString();
            return files.TryGetValue(k, out var value) ?
            value.GetValue<string>(null)! : k;
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
    private WebDriver WebDriver { get; } = webDriver;

    #endregion
    #endregion
    #region 构造函数
    #endregion
}
