
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
    public string OS
    {
        get
        {
            if (UserAgent.Contains("Windows NT"))
                return OSName.Windows;
            if (UserAgent.Contains("Android"))
                return OSName.Android;
            if (UserAgent.Contains("iPhone OS"))
                return OSName.IOS;
            if (UserAgent.Contains("Mac OS"))
                return OSName.Mac;
            return OSName.Unknown;
        }
    }
    #endregion
    #region 硬件类型
    public string HardwareType
        => OS switch
        {
            OSName.Windows or OSName.Mac => HardwareTypeName.PC,
            OSName.Android or OSName.IOS => HardwareTypeName.Phone,
            _ => HardwareTypeName.Unknown
        };
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
