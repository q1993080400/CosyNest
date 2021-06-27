using System.Net;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Http请求的响应
    /// </summary>
    public interface IHttpResponse
    {
        #region 有关响应状态
        #region 获取响应状态
        /// <summary>
        /// 获取Http响应的状态
        /// </summary>
        HttpStatusCode Status { get; }
        #endregion
        #region 响应是否成功
        /// <summary>
        /// 获取响应是否成功
        /// </summary>
        bool IsSuccess
            => (int)Status is >= 200 and <= 299;
        #endregion
        #endregion
        #region 关于响应的结果
        #region 响应的标头
        /// <summary>
        /// 获取Http响应的标头
        /// </summary>
        IHttpHeaderResponse Header { get; }
        #endregion
        #region 响应的内容
        /// <summary>
        /// 获取响应的内容
        /// </summary>
        IHttpContent Content { get; }
        #endregion
        #endregion
    }
}
