using System.IOFrancis.FileSystem;
using System.Office.Realize;

namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 在实现<see cref="IExcelBook"/>时，可以继承本类型，
    /// 以减少重复的工作
    /// </summary>
    public abstract class ExcelBook : OfficeFile, IExcelBook
    {
        #region 返回IExcelBook接口
        /// <summary>
        /// 返回这个对象的接口形式，
        /// 它可以用来访问一些显式实现的成员
        /// </summary>
        protected IExcelBook Interface => this;
        #endregion
        #region 关于工作簿
        #region 返回打印对象
        public abstract IOfficePrint Print { get; }
        #endregion
        #region 开启或关闭自动计算
        public virtual bool AutoCalculation
        {
            get => false;
            set
            {

            }
        }
        #endregion
        #endregion
        #region 返回工作表的容器
        public abstract IExcelSheetCollection Sheets { get; }
        #endregion
        #region 构造函数与创建对象
        #region 提取工作簿
        /// <summary>
        /// 通过路径获取工作簿，不会引发路径被占用的异常
        /// </summary>
        /// <param name="path">工作簿的路径</param>
        /// <param name="delegate">如果缓存中没有使用该路径的工作簿，
        /// 则通过这个委托创建新工作簿，参数就是路径</param>
        /// <returns></returns>
        public static IExcelBook GetExcelsBook(PathText path, Func<PathText, ExcelBook> @delegate)
            => GetOfficeFile(path, @delegate);
        #endregion
        #region 构造函数：指定路径和受支持文件类型
        /// <summary>
        /// 用指定的文件路径初始化Excel工作簿
        /// </summary>
        /// <param name="path">工作簿所在的文件路径，
        /// 如果不是通过文件创建的，则为<see langword="null"/></param>
        /// <param name="supported">该Excel对象所支持的文件类型</param>
        public ExcelBook(PathText? path, IFileType supported)
            : base(path, supported)
        {

        }
        #endregion
        #endregion
    }
}
