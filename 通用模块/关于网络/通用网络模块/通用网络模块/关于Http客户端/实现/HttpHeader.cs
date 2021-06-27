using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace System.NetFrancis.Http.Realize
{
    /// <summary>
    /// 这个记录是Http标头的可选基类
    /// </summary>
    public abstract record HttpHeader : IHttpHeader
    {
        #region 关于标头属性
        #region 说明文档
        /*问：预定义属性和自定义属性指的是什么？有什么区别？
          答：预定义属性指在IHttpHeader的派生接口中定义，
          强类型的标头属性，例如HttpHeaderRequest.Authentication，
          它的类型是AuthenticationHeaderValue，
          自定义属性指除此之外的其他所有标头属性，它们只能以纯文本的形式存在
        
          问：为什么要区分预定义属性和自定义属性？
          答：因为纯文本的标头属性需要解析，比较麻烦，因此作者设计了预定义属性，
          但是根据Http标准，报文标头本身是可定制的，因此自定义属性也需要被支持，
          根据规范，如果预定义属性和自定义属性存在重复，应该以前者为准，
          因为作者希望大家尽量使用强类型的预定义属性，这样可以减少不必要的错误*/
        #endregion
        #region 枚举预定义标头属性
        /// <summary>
        /// 枚举所有预定义标头属性
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<(string Name, string? Value)> Predefined();
        #endregion
        #region 枚举自定义标头属性
        /// <summary>
        /// 枚举所有自定义标头属性
        /// </summary>
        public IEnumerable<(string Name, string Value)> Custom { get; init; }
        #endregion
        #region 索引所有标头属性
        public IReadOnlyDictionary<string, string> Headers()
            => Custom.Union(false, Predefined().Where(x => !x.Value.IsVoid())!).ToDictionary(false);
        #endregion
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => Headers().Join(x => $"{x.Key}:{x.Value}", Environment.NewLine);
        #endregion
        #region 构造函数
        #region 无参数构造函数
        public HttpHeader()
        {
            Custom = Array.Empty<(string, string)>();
        }
        #endregion
        #region 传入标头集合
        /// <summary>
        /// 使用指定的标头集合初始化对象，
        /// 这个构造函数可以使<see cref="HttpHeaders"/>及其派生类能够更方便的转换为<see cref="IHttpHeader"/>
        /// </summary>
        /// <param name="Headers">使用指定的自定义标头集合初始化对象</param>
        public HttpHeader(IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers)
        {
            Custom = Headers.Select(x => (x.Key, x.Value.Join(","))).ToArray();
        }
        #endregion
        #endregion
    }
}
