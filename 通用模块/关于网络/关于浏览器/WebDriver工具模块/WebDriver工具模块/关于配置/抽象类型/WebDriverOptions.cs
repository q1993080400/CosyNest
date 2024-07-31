using System.SafetyFrancis.Authentication;

using OpenQA.Selenium;

namespace System.NetFrancis.Browser;

/// <summary>
/// 这个类型是WebDriver浏览器的选项
/// </summary>
public abstract record WebDriverOptions
{
    #region 公开成员
    #region 是否最大化启动
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该最大化启动浏览器
    /// </summary>
    public bool Maximize { get; init; } = true;
    #endregion
    #region 超时时间
    /// <summary>
    /// 获取未找到元素时的超时时间
    /// </summary>
    public TimeSpan TimeOut { get; init; } = TimeSpan.FromSeconds(3);
    #endregion
    #region 代理
    /// <summary>
    /// 获取浏览器所使用的代理
    /// </summary>
    public UnsafeWebCredentials? Proxy { get; init; }
    #endregion
    #endregion
    #region 生成浏览器配置
    /// <summary>
    /// 生成一个浏览器配置
    /// </summary>
    /// <returns></returns>
    internal abstract DriverOptions GenerateBrowserOptions();
    #endregion
}
