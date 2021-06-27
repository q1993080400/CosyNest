namespace System.Design
{
    /// <summary>
    /// 代表一个不需要释放的<see cref="IDisposablePro"/>，
    /// 它的<see cref="IDisposable.Dispose"/>方法不执行任何操作
    /// </summary>
    public abstract class WithoutRelease : IDisposablePro
    {
        #region 说明文档
        /*问：本类型的Dispose方法不执行任何操作，
          那么它有什么作用？
          答：它适用于这种情况：派生类是完全的托管类型，
          实际上不需要实现IDisposablePro，但是为了实现某些接口，
          必须实现IDisposablePro*/
        #endregion
        #region 指示对象是否可用
        public bool IsAvailable => true;
        #endregion
        #region 释放对象
#pragma warning disable CA1816
        public void Dispose()
        {

        }
#pragma warning restore
        #endregion
    }
}
