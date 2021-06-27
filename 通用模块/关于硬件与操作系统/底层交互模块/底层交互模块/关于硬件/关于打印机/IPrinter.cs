namespace System.Underlying
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个打印机
    /// </summary>
    public interface IPrinter
    {
        #region 打印机的名称
        /// <summary>
        /// 获取打印机的名称
        /// </summary>
        string Name { get; }
        #endregion
    }
}
