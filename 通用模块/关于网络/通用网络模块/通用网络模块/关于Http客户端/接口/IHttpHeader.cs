using System.Collections.Generic;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Http标头
    /// </summary>
    public interface IHttpHeader
    {
        #region 索引所有标头属性
        /// <summary>
        /// 获取一个索引所有标头属性的字典，
        /// 它的键是属性的名称，值是属性的值
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<string, string> Headers();
        #endregion
    }
}
