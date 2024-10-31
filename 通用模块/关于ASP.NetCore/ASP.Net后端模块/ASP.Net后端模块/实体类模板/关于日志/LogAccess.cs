using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个实体类是一个访问日志的模板
/// </summary>
public class LogAccess : Entity, IWithDate
{
    #region 访问时间
    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    #endregion
    #region 用户代理字符串
    /// <summary>
    /// 获取用户代理字符串
    /// </summary>
    public string UserAgent { get; set; } = "";
    #endregion
    #region 用户的IP
    /// <summary>
    /// 获取用户的IP
    /// </summary>
    public string IP { get; set; } = "";
    #endregion
    #region 用户访问的Uri
    /// <summary>
    /// 获取用户访问的Uri
    /// </summary>
    public string Uri { get; set; } = "";
    #endregion
}
