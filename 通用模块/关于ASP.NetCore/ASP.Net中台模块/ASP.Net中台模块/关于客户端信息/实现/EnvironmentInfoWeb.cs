using System.Underlying;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// 该类型是<see cref="IEnvironmentInfo"/>的实现，
/// 可以视为一个Web客户端硬件信息
/// </summary>
sealed class EnvironmentInfoWeb : IEnvironmentInfoWeb
{
    #region 用户代理字符串
    public string UserAgent { get; }
    #endregion
    #region 操作系统
    public OS OS { get; }
    #endregion
    #region 硬件类型
    public HardwareType HardwareType { get; }
    #endregion
    #region 浏览器
    public Browser Browser { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的用户代理字符串初始化对象
    /// </summary>
    /// <param name="userAgent">用户代理字符串</param>
    public EnvironmentInfoWeb(string userAgent)
    {
        UserAgent = userAgent;
        #region 获取OS的本地函数
        static OS GetOS(string userAgent)
        {
            if (userAgent.Contains("Windows NT"))
                return OS.Windows;
            if (userAgent.Contains("Android"))
                return OS.Android;
            if (userAgent.Contains("iPhone OS"))
                return OS.IOS;
            if (userAgent.Contains("Mac OS"))
                return OS.Mac;
            return OS.Other;
        }
        #endregion
        OS = GetOS(userAgent);
        #region 获取硬件类型的本地函数
        static HardwareType GetHardwareType(OS os)
        => os switch
        {
            OS.Windows or OS.Mac => HardwareType.PC,
            OS.Android or OS.IOS => HardwareType.Phone,
            _ => HardwareType.Other
        };
        #endregion
        HardwareType = GetHardwareType(OS);
        #region 获取浏览器的本地函数
        static Browser GetBrowser(string userAgent, OS os)
        {
            var comparer = StringComparison.InvariantCultureIgnoreCase;
            if (userAgent.Contains("Firefox", comparer))
                return Browser.Firefox;
            if (userAgent.Contains("Edg", comparer))
                return Browser.Edge;
            if (userAgent.Contains("MicroMessenger", comparer))
                return Browser.WeChat;
            if (userAgent.Contains("QQBrowser", comparer))
                return Browser.QQ;
            if (userAgent.Contains("Chrome", comparer))
                return Browser.Chrome;
            if (os is OS.IOS or OS.Mac)
                return Browser.Safari;
            return Browser.Other;
        }
        #endregion
        Browser = GetBrowser(userAgent, OS);
    }
    #endregion
}
