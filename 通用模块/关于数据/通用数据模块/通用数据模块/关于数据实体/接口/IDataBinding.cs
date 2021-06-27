namespace System.DataFrancis
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以将数据与数据源绑定
    /// </summary>
    public interface IDataBinding
    {
        #region 数据通知数据源
        #region 说明文档
        /*问：向数据源发出通知很可能是一个耗时的方法，
          那么这些API为什么不设计为异步？
          答：事实上，它们已经是异步方法了，
          因为void方法也可以作为异步方法，只是无法等待而已，
          但是作者认为它们也不需要等待，因为数据被修改后的效果已经在客户端生效，
          等待服务端更新完成意义不大*/
        #endregion
        #region 通知修改
        /// <summary>
        /// 当数据发生修改时，可以调用这个方法通知数据源
        /// </summary>
        /// <param name="columnName">发生修改的列名</param>
        /// <param name="newValue">修改后的新值</param>
        void NoticeUpdateToSource(string columnName, object? newValue);
        #endregion
        #region 通知删除
        /// <summary>
        /// 当数据被删除时，可以调用这个方法通知数据源
        /// </summary>
        void NoticeDeleteToSource();
        #endregion
        #endregion
        #region 数据源通知数据
        #region 说明文档
        /*实现本API请遵循以下规范：
          如果不支持数据源通知数据，
          那么就声明两个Add和Remove访问器为空的事件，而不是抛出异常
          因为在设置IData.Binding属性时会注册这两个事件，
          抛出异常会导致程序崩溃*/
        #endregion
        #region 通知修改
        /// <summary>
        /// 当数据源发生修改时，会调用这个事件通知数据，
        /// 事件的参数分别是发生修改的列名，以及修改后的新值
        /// </summary>
        event Action<string, object?>? NoticeUpdateToData;
        #endregion
        #region 通知删除
        /// <summary>
        /// 当数据源删除数据时，会调用这个事件通知数据
        /// </summary>
        event Action? NoticeDeleteToData;
        #endregion
        #endregion 
    }
}
