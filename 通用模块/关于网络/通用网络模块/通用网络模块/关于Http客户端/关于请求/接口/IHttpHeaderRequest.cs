using System.Net.Http.Headers;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Http请求头
    /// </summary>
    public interface IHttpHeaderRequest : IHttpHeader
    {
        #region 获取身份验证标头
        /// <summary>
        /// 获取Authentication标头值，
        /// 它用于身份验证
        /// </summary>
        AuthenticationHeaderValue? Authentication { get; }
        #endregion
    }
}
