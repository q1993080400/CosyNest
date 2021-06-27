using System.Net;
using System.Net.Http;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 这个类型是<see cref="IHttpResponse"/>的实现，
    /// 可以视为一个Http请求的响应
    /// </summary>
    class HttpResponse : IHttpResponse
    {
        #region 响应的状态
        public HttpStatusCode Status { get; }
        #endregion
        #region 响应的标头
        public IHttpHeaderResponse Header { get; }
        #endregion
        #region 响应的正文
        public IHttpContent Content { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的Http消息初始化对象
        /// </summary>
        /// <param name="message">指定的Http消息</param>
        public HttpResponse(HttpResponseMessage message)
        {
            Status = message.StatusCode;
            Header = new HttpHeaderResponse(message.Headers);
            Content = new HttpContentFrancis(message.Content, false);
        }
        #endregion
    }
}
