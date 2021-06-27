using System.Collections.Generic;
using System.Net.Http;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 这个类型是<see cref="IHttpContent"/>的实现，
    /// 可以视为一个Http消息的内容
    /// </summary>
    public record HttpContentFrancis : IHttpContent
    {
        #region Http标头
        IHttpHeaderContent IHttpContent.Header
            => Header;

        /// <summary>
        /// 返回Http内容的标头
        /// </summary>
        public HttpHeaderContent Header { get; init; }
        #endregion
        #region Http内容
        /// <summary>
        /// 返回Http消息的内容，
        /// 它以二进制的格式呈现
        /// </summary>
        public IEnumerable<byte> Content { get; init; }
        #endregion
        #region 构造函数
        #region 无参数构造函数
        public HttpContentFrancis()
        {
            Content = Array.Empty<byte>();
            Header = new();
        }
        #endregion
        #region 复制HttpContent的内容
        /// <summary>
        /// 使用指定的Http内容初始化对象
        /// </summary>
        /// <param name="content">指定的Http内容</param>
        /// <param name="autoRelease">如果这个参数为<see langword="true"/>，
        /// 则在构造函数执行完毕后，还会自动释放掉<paramref name="content"/></param>
        public HttpContentFrancis(HttpContent content, bool autoRelease)
        {
            Content = content.ReadAsByteArrayAsync().Result;
            Header = new(content.Headers);
            if (autoRelease)
                content.Dispose();
        }
        #endregion 
        #endregion
    }
}
