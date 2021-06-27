using System.Net.Http;
using System.Threading.Tasks;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 这个类型是<see cref="IHttpClient"/>的实现，
    /// 可以用来发起Http请求
    /// </summary>
    class HttpClientRealize : IHttpClient
    {
        #region 封装的HttpClient对象
        /// <summary>
        /// 获取封装的<see cref="Net.Http.HttpClient"/>对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private HttpClient HttpClient { get; }
        #endregion
        #region 发起Http请求
        public async Task<IHttpResponse> Request(IHttpRequestRecording request)
        {
            using var r = request.ToHttpRequestMessage();
            using var response = await HttpClient.SendAsync(r);
            return new HttpResponse(response);
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的Http客户端初始化对象
        /// </summary>
        /// <param name="httpClient">指定的Http客户端，
        /// 本对象的功能就是通过它实现的</param>
        public HttpClientRealize(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }
        #endregion
    }
}
