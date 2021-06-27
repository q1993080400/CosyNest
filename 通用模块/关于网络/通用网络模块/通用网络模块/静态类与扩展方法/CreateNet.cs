using System.Net.Http.Json;
using System.NetFrancis.Http;
using System.Text.Json;

namespace System.NetFrancis
{
    /// <summary>
    /// 这个静态类可以用来创建和网络相关的对象
    /// </summary>
    public static class CreateNet
    {
        #region 返回公用的IHttpClient对象
        private static IHttpClient? HttpClientField;

        /// <summary>
        /// 返回一个公用的<see cref="IHttpClient"/>对象，
        /// 它在整个程序中是单例的，适合在客户端应用程序中使用
        /// </summary>
        public static IHttpClient HttpClient
            => HttpClientField ??= new HttpClientRealize(new());
        #endregion
        #region 有关IHttpContent
        #region 使用Json创建
        /// <summary>
        /// 将指定对象序列化，
        /// 然后创建一个包含Json的<see cref="HttpContentFrancis"/>
        /// </summary>
        /// <typeparam name="Obj">要序列化的对象的类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="options">控制序列化过程的选项</param>
        /// <returns></returns>
        public static HttpContentFrancis HttpContentJson<Obj>(Obj? obj, JsonSerializerOptions? options = null) 
            => new(JsonContent.Create(obj, options: options), true);
        #endregion
        #endregion
    }
}
