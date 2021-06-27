using System.Linq.Expressions;
using System.Threading.Tasks;

namespace System.DataFrancis
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个双向的数据管道，
    /// 它既可以查询数据又可以添加数据
    /// </summary>
    public interface IDataPipe : IDataPipeAdd, IDataPipeQuery
    {
        #region 获取是否支持绑定
        /// <inheritdoc cref="IDataPipeQuery.CanBinding"/>
        new bool CanBinding => false;
        #endregion
        #region 删除数据
        #region 说明文档
        /*问：按照通常的思维习惯，删除数据和添加数据是相对应的，
          这个功能应该放在IDataPipeAdd中，那么为什么会放在这里？
          答：因为这个API的功能是“删除符合指定条件的数据”，
          因此它需要先进行一次查询操作，不能查询数据的IDataPipeAdd不适合加入这个API
        
          问：既然如此，如果要支持删除数据，是否必须实现IDataPipe接口？
          答：不是，即便没有实现IDataPipe接口，通过IDataPipeAdd和IDataPipeQuery获取到的数据，
          仍然可以通过直接调用IData.Delete()来删除数据，但是这种方法必须先获取数据，后删除，性能较低*/
        #endregion
        #region 正式方法
        /// <summary>
        /// 直接从数据源中删除符合指定谓词的数据，不返回结果集
        /// </summary>
        /// <param name="expression">一个用来指定删除条件的表达式</param>
        Task Delete(Expression<Func<PlaceholderData, bool>> expression);
        #endregion
        #endregion
    }
}
