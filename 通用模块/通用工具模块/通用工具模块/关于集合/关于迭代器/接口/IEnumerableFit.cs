namespace System.Collections.Generic
{
    /// <summary>
    /// 凡是实现这个接口的类型，都可以作为一个全能的迭代器，
    /// 它同时实现了<see cref="IEnumerable{T}"/>和<see cref="IAsyncEnumerable{T}"/>
    /// </summary>
    /// <typeparam name="Obj">迭代器的元素类型</typeparam>
    public interface IEnumerableFit<out Obj> : IEnumerable<Obj>, IAsyncEnumerable<Obj>
    {
        #region 说明文档
        /*问：为什么需要这个接口？
          答：因为IEnumerable和IAsyncEnumerable的功能非常相似，
          除了一个同步一个异步以外没有任何区别，但是却需要为它们维护两套不同的API，
          这非常麻烦，事实上作者认为这是一个设计失误，IAsyncEnumerable本来就应该实现IEnumerable，
          就像IEnumerable<T>应该实现IEnumerable一样
        
          问：IEnumerable，IAsyncEnumerable，IEnumerableFit这三个接口应该如何取舍？
          在什么样的情况下应该使用它们？
          答：请遵循以下规范：
          #Linq以及用来扩展Linq的扩展方法应该返回IEnumerableFit
        
          #返回IAsyncEnumerable的方法和属性，在一般情况下，应该重构为返回IEnumerableFit
        
          这样带来的好处是：
          #同时支持同步迭代和异步迭代
          #可以同时使用传入IEnumerable和IAsyncEnumerable的API（主要是Linq）*/
        #endregion
    }
}
