using System.Threading.Tasks;

namespace System.Design.Async
{
    /// <summary>
    /// 该类型是<see cref="IAsyncPropertyP1{Value}"/>的实现，
    /// 它使用委托来读写异步属性
    /// </summary>
    /// <typeparam name="Value">异步属性的值的类型</typeparam>
    class AsyncProperty<Value> : IAsyncProperty<Value>
    {
        #region 读取异步属性
        #region 正式属性
        public Task<Value> Get
            => GetDelegate();
        #endregion
        #region 委托
        /// <summary>
        /// 这个委托用于读取异步属性
        /// </summary>
        private Func<Task<Value>> GetDelegate { get; }
        #endregion
        #endregion
        #region 写入异步属性
        #region 正式方法
        public Task Set(Value value)
            => SetDelegate(value);
        #endregion
        #region 委托
        /// <summary>
        /// 这个委托用于写入异步属性
        /// </summary>
        private Func<Value, Task> SetDelegate { get; }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="getDelegate">用于读取异步属性的委托</param>
        /// <param name="setDelegate">用于写入异步属性的委托</param>
        public AsyncProperty(Func<Task<Value>> getDelegate, Func<Value, Task> setDelegate)
        {
            this.GetDelegate = getDelegate;
            this.SetDelegate = setDelegate;
        }
        #endregion
    }
}
