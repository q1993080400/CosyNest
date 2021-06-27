using System.Net.Http;
using System.Net.Http.Headers;
using System.NetFrancis.Http;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 有关网络的扩展方法全部放在这里
    /// </summary>
    public static class ExtenNet
    {
        #region 有关IHttpClient及其衍生类型
        #region 将HttpClient转换为IHttpClient
        /// <summary>
        /// 返回一个<see cref="HttpClient"/>的<see cref="IHttpClient"/>包装
        /// </summary>
        /// <param name="httpClient">待包装的<see cref="HttpClient"/></param>
        /// <returns></returns>
        public static IHttpClient ToHttpClient(this HttpClient httpClient)
            => new HttpClientRealize(httpClient);
        #endregion
        #region 将HttpRequestRecording转换为HttpRequestMessage
        /// <summary>
        /// 将<see cref="HttpRequestRecording"/>转换为等效的<see cref="HttpRequestMessage"/>
        /// </summary>
        /// <param name="recording">待转换的<see cref="HttpRequestRecording"/></param>
        /// <returns></returns>
        internal static HttpRequestMessage ToHttpRequestMessage(this IHttpRequestRecording recording)
        {
            var m = new HttpRequestMessage()
            {
                RequestUri = new(recording.UriComplete),
                Method = recording.HttpMethod,
                Content = recording.Content.ToHttpContent()
            };
            recording.Header.CopyHeader(m.Headers);
            return m;
        }
        #endregion
        #region 将IHttpContent转换为HttpContent
        /// <summary>
        /// 将<see cref="IHttpContent"/>转换为<see cref="HttpContent"/>
        /// </summary>
        /// <param name="content">待转换的<see cref="HttpContent"/></param>
        /// <returns></returns>
        private static HttpContent? ToHttpContent(this IHttpContent? content)
        {
            if (content is null)
                return null;
            var arryContent = new ByteArrayContent(content.Content.ToArray());
            content.Header.CopyHeader(arryContent.Headers);
            return arryContent;
        }
        #endregion
        #region 将IHttpHeader的标头复制到HttpHeaders
        /// <summary>
        /// 将<see cref="IHttpHeader"/>的所有标头复制到另一个<see cref="HttpHeaders"/>中
        /// </summary>
        /// <param name="header">待复制标头的<see cref="IHttpHeader"/></param>
        /// <param name="bclHeader"><paramref name="header"/>的所有标头将被复制到这个参数中</param>
        private static void CopyHeader(this IHttpHeader header, HttpHeaders bclHeader)
        {
            foreach (var (key, value) in header.Headers())
            {
                bclHeader.Add(key, value);
            }
        }
        #endregion
        #endregion
        #region 与Base64互相转换
        #region 转换为Base64
        /// <summary>
        /// 将字符串转换为Base64形式
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">指定将字符串编码为何种形式，再转换为Base64，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <returns></returns>
        public static string ToBase64(this string text, Encoding? encoding = null)
            => Convert.ToBase64String(text.ToBytes(encoding));
        #endregion
        #region 从Base64转换
        /// <summary>
        /// 将Base64形式的字符串解码，然后将其转换为字符串
        /// </summary>
        /// <param name="text">要转换的字符串</param>
        /// <param name="encoding">指定将解码后的字节数组转换为字符串的编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <returns></returns>
        public static string FromBase64(this string text, Encoding? encoding = null)
            => (encoding ?? Encoding.UTF8).GetString(Convert.FromBase64String(text));
        #endregion
        #endregion
    }
}
