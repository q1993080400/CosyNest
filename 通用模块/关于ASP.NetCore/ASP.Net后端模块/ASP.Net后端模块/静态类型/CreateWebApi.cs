using Microsoft.AspNetCore.Http.Extensions;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以用来创建有关WebApi的对象
/// </summary>
public static class CreateWebApi
{
    #region 创建访问日志
    /// <summary>
    /// 创建一个访问日志
    /// </summary>
    /// <typeparam name="Log">访问日志的类型</typeparam>
    /// <param name="http">当前访问的Http上下文对象</param>
    /// <returns></returns>
    public static Log LogAccess<Log>(HttpContext http)
        where Log : LogAccess, new()
    {
        var request = http.Request;
        return new Log()
        {
            UserAgent = request.Headers.UserAgent.ToString() ?? "",
            IP = http.Connection.RemoteIpAddress?.ToString() ?? "",
            Uri = request.GetEncodedUrl()
        };
    }
    #endregion
}
