
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
    #region 操作系统的名称
    public OS OS
    {
        get
        {
            if (UserAgent.Contains("Windows NT"))
                return OS.Windows;
            if (UserAgent.Contains("Android"))
                return OS.Android;
            if (UserAgent.Contains("iPhone OS"))
                return OS.IOS;
            if (UserAgent.Contains("Mac OS"))
                return OS.Mac;
            return OS.Other;
        }
    }
    #endregion
    #region 硬件类型
    public HardwareType HardwareType
        => OS switch
        {
            OS.Windows or OS.Mac => HardwareType.PC,
            OS.Android or OS.IOS => HardwareType.Phone,
            _ => HardwareType.Other
        };
    #endregion
    #region 浏览器
    public Browser Browser
    {
        get
        {
            if (UserAgent.Contains("MicroMessenger"))
                return Browser.WeChat;
            if (UserAgent.Contains("QQ"))
                return Browser.QQ;
            return Browser.Other;
        }
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的用户代理字符串初始化对象
    /// </summary>
    /// <param name="userAgent">用户代理字符串</param>
    public EnvironmentInfoWeb(string userAgent)
    {
        this.UserAgent = userAgent;
    }
    #endregion
}
