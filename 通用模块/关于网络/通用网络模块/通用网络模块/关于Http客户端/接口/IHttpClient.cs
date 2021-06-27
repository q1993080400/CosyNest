using System.Threading.Tasks;
using System.TreeObject;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来发起Http请求
    /// </summary>
    public interface IHttpClient
    {
        #region 说明文档
        /*问：BCL有原生的HttpClient，
          既然如此，为什么需要本类型？
          答：因为原生的HttpClient是可变的，
          而且经常在多个线程甚至整个应用中共享同一个HttpClient对象，
          这种操作非常危险，而本接口的所有API都是纯函数，消除了这个问题

          同时，本接口使用IHttpRequestRecording来封装提交Http请求的信息，
          根据推荐做法，通常使用该接口的实现HttpRequestRecording，
          它是一个记录，通过C#9.0新支持的with表达式，能够更加方便的替换请求内容的任何部分*/
        #endregion
        #region 发起Http请求
        #region 直接返回IHttpResponse
        /// <summary>
        /// 发起Http请求，并返回结果
        /// </summary>
        /// <param name="request">请求消息的内容</param>
        /// <returns></returns>
        Task<IHttpResponse> Request(IHttpRequestRecording request);
        #endregion
        #region 返回文本
        /// <summary>
        /// 发起Http请求，并以文本格式返回结果
        /// </summary>
        /// <param name="request">请求消息的内容</param>
        /// <returns></returns>
        async Task<string> RequestText(IHttpRequestRecording request)
             => (await Request(request)).Content.ToText();
        #endregion
        #region 返回反序列化后的对象
        /// <summary>
        /// 发起Http请求，并将结果反序列化，然后返回
        /// </summary>
        /// <typeparam name="Obj">反序列化的返回类型</typeparam>
        /// <param name="request">请求消息的内容</param>
        /// <param name="serialization">用于反序列化的对象，
        /// 如果为<see langword="null"/>，则使用默认对象</param>
        /// <returns></returns>
        async Task<Obj?> RequestObject<Obj>(IHttpRequestRecording request, ISerialization? serialization = null)
            => (await Request(request)).Content.ToObject<Obj>(serialization);
        #endregion
        #endregion
    }
}
