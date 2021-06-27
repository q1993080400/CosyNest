using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.DataFrancis
{
    /// <summary>
    /// 所有实现这个接口的类型，
    /// 都可以作为一个支持查询数据的管道
    /// </summary>
    public interface IDataPipeQuery
    {
        #region 说明文档
        /*问：DataQuery方法只能传入一个输入为PlaceholderData，输出为Bool的表达式，
          那么如果数据是通过命令（例如存储过程）获得的，应该怎么表示？
          答：你可以实例化一个实现IDataPipeQuery的存储过程对象，
          并将它的参数填好，然后执行Query方法，Expression可以填入任意值，
          这样就可以表示通过命令的查询
        
          问：如何表示类似“以Age为键进行排序，然后取前500条数据”这样的查询条件？
          答：排序这个功能在设计上由数据源负责，换言之，数据源应该直接提供已经排序好的数据，
          然后“取前500条数据”用表达式“x=>x.Index<500”表示，x的类型是PlaceholderData*/
        #endregion
        #region 辅助方法
        #region 转换迭代器
        /// <summary>
        /// 将一个枚举<see cref="IData"/>的异步迭代器，
        /// 转换为等效的枚举<see cref="PlaceholderData"/>的同步迭代器
        /// </summary>
        /// <param name="datas">待转换的异步迭代器</param>
        /// <returns></returns>
        private static IEnumerable<PlaceholderData> Convert(IAsyncEnumerable<IData> datas)
            => datas.Fit().PackIndex().
            Select(x => new PlaceholderData(x.Elements, x.Index));
        #endregion
        #region 筛选查询结果
        /// <summary>
        /// 不使用表达式树，而是直接执行委托来筛选查询结果
        /// </summary>
        /// <param name="datas">待筛选的数据集</param>
        /// <param name="expression">用来筛选数据的表达式，
        /// 它会被翻译为一个传入<see cref="IData"/>的等价表达式</param>
        /// <returns>如果<paramref name="expression"/>为<see langword="null"/>，则返回<paramref name="datas"/>，
        /// 否则将表达式翻译为传入<see cref="IData"/>的表达式，并筛选数据，返回结果</returns>
        protected static IAsyncEnumerable<IData> QueryDirect(IAsyncEnumerable<IData> datas, Expression<Func<PlaceholderData, bool>>? expression)
            => expression is null ?
            datas :
            Convert(datas).Where(expression.Compile()).
            Select(x => x.Data).Fit();
        #endregion
        #endregion
        #region 获取是否支持绑定
        /// <summary>
        /// 如果这个值为<see langword="true"/>，代表该数据管道支持绑定
        /// </summary>
        bool CanBinding => false;
        #endregion
        #region 统计数据
        #region 说明文档
        /*问：统计数据的API有什么作用？
          和查询数据的API有何不同？
          答：作用在于可以返回一些不是IData的数据，举例说明，
          如果想要获取数据的总数，此时就应该使用统计API，
          如果先查询所有数据，然后再调用Sum()，可能在性能上会有严重损失*/
        #endregion
        #region 正式方法
        /// <summary>
        /// 对数据进行统计，并返回统计结果
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="expression">用来统计数据的表达式，参数就是抽象的数据集</param>
        /// <returns></returns>
        Task<Ret> Statistics<Ret>(Expression<Func<IAsyncEnumerable<PlaceholderData>, Task<Ret>>> expression)
            => expression.Compile().Invoke
            (Convert(Query()).Fit());
        #endregion
        #endregion
        #region 查询数据
        /// <summary>
        /// 通过管道查询数据，并返回结果集
        /// </summary>
        /// <param name="expression">一个用来指定查询条件的表达式，如果数据源不支持查询或不需要查询，则为<see langword="null"/></param>
        /// <param name="binding">如果这个值为<see langword="true"/>，
        /// 则在查询数据的时候，还会将数据与数据源绑定，但数据源可能不支持绑定</param>
        /// <returns></returns>
        IEnumerableFit<IData> Query(Expression<Func<PlaceholderData, bool>>? expression = null, bool binding = false);
        #endregion
    }
}
