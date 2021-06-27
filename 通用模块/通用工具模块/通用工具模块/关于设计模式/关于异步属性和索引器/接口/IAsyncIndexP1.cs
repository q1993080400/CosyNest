using System.Threading.Tasks;

namespace System.Design.Async
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个拥有一个参数的异步索引器
    /// </summary>
    /// <typeparam name="P1">索引器的第一个参数</typeparam>
    /// <typeparam name="Value">索引器的类型</typeparam>
    public interface IAsyncIndex<P1, Value>
    {
        #region 读取索引器
        /// <summary>
        /// 读取异步索引器
        /// </summary>
        /// <param name="p1">索引器的第一个参数</param>
        /// <returns></returns>
        Task<Value> Get(P1 p1);
        #endregion
        #region 写入索引器
        /// <summary>
        /// 写入异步索引器
        /// </summary>
        /// <param name="p1">索引器的第一个参数</param>
        /// <param name="value">待写入的值</param>
        /// <returns></returns>
        Task Set(P1 p1, Value value);
        #endregion
    }
}
