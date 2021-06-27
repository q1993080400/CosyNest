using System.Collections.Generic;
using System.Net.Http.Headers;
using System.NetFrancis.Http.Realize;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 这个记录代表Http响应的标头
    /// </summary>
    record HttpHeaderResponse : HttpHeader, IHttpHeaderResponse
    {
        #region 枚举预定义标头属性
        protected override IEnumerable<(string Name, string? Value)> Predefined()
            => Array.Empty<(string, string?)>();
        #endregion
        #region 构造函数
        /// <summary>
        /// 复制一个<see cref="HttpResponseHeaders"/>的所有属性，并初始化对象
        /// </summary>
        /// <param name="header">待复制的响应标头</param>
        public HttpHeaderResponse(HttpResponseHeaders header)
            : base(header)
        {

        }
        #endregion
    }
}
