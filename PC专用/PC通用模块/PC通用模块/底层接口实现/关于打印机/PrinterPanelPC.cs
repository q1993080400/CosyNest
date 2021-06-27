using System.Collections.Generic;
using System.Linq;
using System.Printing;

namespace System.Underlying.PC
{
    /// <summary>
    /// 这个类型是<see cref="IPrinterPanel"/>的实现，
    /// 可以视为一个PC平台上的打印机面板
    /// </summary>
    class PrinterPanelPC : IPrinterPanel
    {
        #region 封装的打印机对象
        /// <summary>
        /// 获取封装的打印服务器对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private LocalPrintServer PackPrinter { get; }
        = new LocalPrintServer();
        #endregion
        #region 索引所有打印机
        public IReadOnlyDictionary<string, IPrinter> Printers { get; }
        #endregion
        #region 默认打印机
        public IPrinter DefaultPrinter
        {
            get => new PrinterPC(PackPrinter.DefaultPrintQueue);
            set => PackPrinter.DefaultPrintQueue = value.To<PrinterPC>().PackPrinter;
        }
        #endregion
        #region 构造函数
        public PrinterPanelPC()
        {
            Printers = PackPrinter.GetPrintQueues().
                ToDictionary(x => (x.Name, (IPrinter)new PrinterPC(x)), true);
        }
        #endregion
    }
}
