using System.Printing;

namespace System.Underlying.PC
{
    /// <summary>
    /// 这个类型是<see cref="IPrinter"/>的实现，
    /// 可以视为一个PC平台上的打印机
    /// </summary>
    class PrinterPC : IPrinter
    {
        #region 封装的打印机对象
        /// <summary>
        /// 获取封装的打印机对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal PrintQueue PackPrinter { get; }
        #endregion
        #region 获取打印机的名称
        public string Name
            => PackPrinter.Name;
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => Name;
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的打印机初始化对象
        /// </summary>
        /// <param name="packPrinter">指定的打印机</param>
        public PrinterPC(PrintQueue packPrinter)
        {
            this.PackPrinter = packPrinter;
        }
        #endregion
    }
}
