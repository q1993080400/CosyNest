using System.Underlying;

namespace Microsoft.AspNetCore.Http;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来表示Web客户端的基本信息
/// </summary>
public interface IEnvironmentInfoWeb : IEnvironmentInfo
{
    #region 获取用户代理字符串
    /// <summary>
    /// 获取用户代理字符串
    /// </summary>
    string UserAgent { get; }
    #endregion
}
