namespace System.Design
{
    /// <summary>
    /// 这个类型可以在被回收时自动释放非托管资源
    /// </summary>
    public abstract class AutoRelease : IDisposablePro
    {
        #region 说明文档
        /*问：根据Net规范，不推荐在析构函数中释放非托管对象，
          既然如此，为什么本类型要为派生类提供这个功能？
          答：作者对这个问题持有异议，因为内存泄露是一个熵增过程，
          一旦开始，它就只会恶化，不会缓解，因此必须尽最大努力避免，
          析构函数会产生对象老化问题，并且会浪费一点CPU资源，
          但是它付出的代价是暂时的，它能够保证对象最终仍然会被回收

          而且，在手动调用了本对象的Dispose方法以后，
          它会阻止CLR调用析构函数，因此这种做法能够防止菜鸟犯错，
          但是对有经验的开发人员来说，它的代价是不存在的*/
        #endregion
        #region 关于释放对象
        #region 正式方法
        public void Dispose()
        {
            if (IsAvailable)
            {
                IsAvailable = false;
                GC.SuppressFinalize(this);
                DisposeRealize();
            }
        }
        #endregion
        #region 模板方法
        /// <summary>
        /// 释放资源的实际操作在这个函数中
        /// </summary>
        protected abstract void DisposeRealize();
        #endregion
        #endregion
        #region 指示对象是否可用
        public bool IsAvailable { get; private set; } = true;
        #endregion
        #region 析构函数
        /// <summary>
        /// 在本对象被回收时，自动释放非托管资源
        /// </summary>
        ~AutoRelease()
        {
            Dispose();
        }
        #endregion
    }
}
