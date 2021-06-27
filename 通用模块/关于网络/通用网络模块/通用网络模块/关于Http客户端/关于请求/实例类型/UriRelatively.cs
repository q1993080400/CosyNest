using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 该记录可以作为一个相对Uri，
    /// 或作为一个绝对Uri中的相对部分
    /// </summary>
    public record UriRelatively
    {
        #region 隐式类型转换
        public static implicit operator string(UriRelatively uri)
            => uri.UriComplete;
        #endregion
        #region 关于Uri
        #region 获取扩展Uri
        private readonly string UriExtendedField = "";

        /// <summary>
        /// 获取请求的目标的扩展Uri部分，
        /// 通过它可以找到主机上的资源，
        /// 如果不存在，则返回<see cref="string.Empty"/>
        /// </summary>
        public string UriExtended
        {
            get => UriExtendedField;
            init => UriExtendedField = value.Trim('/');
        }
        #endregion
        #region 获取Uri参数
        /// <summary>
        /// 获取一个索引Uri参数的字典
        /// </summary>
        public IReadOnlyDictionary<string, string> UriParameters { get; init; }
            = CreateCollection.EmptyDictionary<string, string>();
        #endregion
        #region 获取完整Uri
        /// <summary>
        /// 获取完整Uri，它可能是绝对或相对的
        /// </summary>
        public virtual string UriComplete
        {
            get
            {
                var uri = new StringBuilder();
                if (UriExtended is not "")
                    uri.Append("/" + UriExtended);
                if (UriParameters.Any())
                    uri.Append("/?" + UriParameters.Join(x => $"{x.Key}={x.Value}", "&"));
                return uri.ToString();
            }
        }
        #endregion
        #endregion 
        #region 重写ToString
        public override string ToString()
            => UriComplete;
        #endregion
    }
}
