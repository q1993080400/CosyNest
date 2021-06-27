using System.Net.Http;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来封装提交Http请求所需要的信息
    /// </summary>
    public interface IHttpRequestRecording
    {
        #region 获取完整Uri
        /// <summary>
        /// 获取完整的绝对Uri
        /// </summary>
        string UriComplete { get; }
        #endregion
        #region 获取请求头
        /// <summary>
        /// 获取Http请求头
        /// </summary>
        IHttpHeaderRequest Header { get; }
        #endregion
        #region 获取Http方法
        /// <summary>
        /// 获取请求的Http方法，默认为Get
        /// </summary>
        HttpMethod HttpMethod { get; }
        #endregion
        #region Http请求内容
        /// <summary>
        /// 获取Http请求的内容
        /// </summary>
        IHttpContent? Content { get; }
        #endregion
    }
}
