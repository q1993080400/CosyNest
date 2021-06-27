namespace System.Underlying
{
    /// <summary>
    /// 这个接口封装了一些关于电源的API
    /// </summary>
    public interface IPower
    {
        #region 关于休眠
        #region 是否允许休眠
        /// <summary>
        /// 如果这个值为<see langword="true"/>，代表允许当前硬件休眠或锁屏，
        /// 否则代表阻止休眠或锁屏
        /// </summary>
        bool CanDormancy { get; set; }
        #endregion
        #region 创建休眠锁
        /// <summary>
        /// 创建一个休眠锁，
        /// 当它被初始化时，阻止计算机休眠，
        /// 当它被释放时，恢复计算机休眠
        /// </summary>
        /// <returns></returns>
        IDisposable CreateDormancyLock()
            => FastRealize.Disposable(
                () => CanDormancy = false,
                () => CanDormancy = true);
        #endregion
        #endregion
        #region 关闭电源
        /// <summary>
        /// 关闭电源，警告：执行本方法需谨慎
        /// </summary>
        void Shutdown();
        #endregion
        #region 重启电源
        /// <summary>
        /// 重新启动硬件，警告：执行本方法需谨慎
        /// </summary>
        void Restart();
        #endregion
    }
}
