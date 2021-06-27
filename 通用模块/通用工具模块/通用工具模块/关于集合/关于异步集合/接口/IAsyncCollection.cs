using System.Linq;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个可以添加删除元素的异步迭代器
    /// </summary>
    /// <typeparam name="Obj">迭代器的元素类型</typeparam>
    public interface IAsyncCollection<Obj> : IAsyncEnumerable<Obj>
    {
        #region 返回元素数量
        /// <summary>
        /// 返回迭代器中元素的数量
        /// </summary>
        Task<int> CountAsync
            => this.Linq(x => x.Count());
        #endregion
        #region 添加元素
        /// <summary>
        /// 向迭代器中添加元素
        /// </summary>
        /// <param name="item">待添加的元素</param>
        /// <returns></returns>
        Task AddAsync(Obj item);
        #endregion
        #region 移除元素
        /// <summary>
        /// 移除迭代器中的元素，
        /// 并返回是否移除成功
        /// </summary>
        /// <param name="item">待移除的元素</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Obj item);
        #endregion
        #region 检查是否包含指定元素
        /// <summary>
        /// 检查这个异步集合是否包含指定元素
        /// </summary>
        /// <param name="item">待检查的元素</param>
        /// <returns>如果包含指定元素，
        /// 则返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        Task<bool> ContainsAsync(Obj item);
        #endregion
        #region 移除全部元素
        /// <summary>
        /// 移除迭代器中的全部元素
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();
        #endregion
    }
}
