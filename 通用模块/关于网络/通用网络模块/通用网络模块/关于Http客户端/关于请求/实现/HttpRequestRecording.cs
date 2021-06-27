using System.Linq;
using System.Net.Http;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 这个记录封装了提交Http请求所需要的信息
    /// </summary>
    public record HttpRequestRecording : IHttpRequestRecording
    {
        #region 获取请求的Uri
        string IHttpRequestRecording.UriComplete => Uri.UriComplete;

        private readonly UriAbsolute UriCompleteField;

        /// <summary>
        /// 获取请求的目标Uri
        /// </summary>
        public UriAbsolute Uri
        {
            get => UriCompleteField;
            init
            {
                if (HttpMethod != HttpMethod.Get && value.UriParameters.Any())
                    throw new NotSupportedException($"只有Get方法才可以在Uri中设置参数");
                UriCompleteField = value;
            }
        }
        #endregion
        #region 获取请求头
        IHttpHeaderRequest IHttpRequestRecording.Header => Header;

        /// <summary>
        /// 获取Http请求头
        /// </summary>
        public HttpHeaderRequest Header { get; init; } = new();
        #endregion
        #region 获取Http方法
        public HttpMethod HttpMethod { get; init; } = HttpMethod.Get;
        #endregion
        #region Http请求内容
        public IHttpContent? Content { get; init; }
        #endregion
        #region 构造函数
#pragma warning disable CS8618
        #region 指定Uri
        /// <summary>
        /// 使用指定的Uri初始化对象
        /// </summary>
        /// <param name="uri"></param>
        public HttpRequestRecording(UriAbsolute uri)
        {
            this.Uri = uri;
        }
        #endregion
        #region 无参数构造函数
        public HttpRequestRecording()
        {
            Uri = new();
        }
        #endregion
#pragma warning restore
        #endregion
    }
}
