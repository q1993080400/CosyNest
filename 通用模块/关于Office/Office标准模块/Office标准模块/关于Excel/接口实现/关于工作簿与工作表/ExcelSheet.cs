using System.Collections.Generic;
using System.DrawingFrancis;
using System.Office.Chart;
using System.DrawingFrancis.Graphics;

namespace System.Office.Excel.Realize
{
    /// <summary>
    ///  在实现<see cref="IExcelSheet"/>时，可以继承自本类型，
    ///  以减少重复的操作
    /// </summary>
    public abstract class ExcelSheet : IExcelSheet
    {
        #region 返回IExcelSheet接口
        /// <summary>
        /// 返回本对象的接口形式，
        /// 通过它可以访问一些显式实现的成员
        /// </summary>
        protected IExcelSheet Interface => this;
        #endregion
        #region 关于单元格
        #region 返回工作表的用户范围
        public abstract IExcelCells RangUser { get; }
        #endregion
        #region 返回行或者列
        public abstract IExcelRC GetRC(int begin, int? end, bool isRow);
        #endregion
        #endregion
        #region 关于工作簿与工作表
        #region 返回工作表所在的工作薄
        public IExcelBook Book { get; }
        #endregion
        #region 获取或设置工作表的名称
        public abstract string Name { get; set; }
        #endregion
        #region 对工作表的操作
        #region 删除工作表
        public abstract void Delete();
        #endregion
        #region 复制工作表
        public abstract IExcelSheet Copy(IExcelSheetCollection collection, Func<string, int, string>? renamed = null);
        #endregion
        #endregion
        #region 返回页面
        public abstract IPageSheet Page { get; }
        #endregion
        #endregion
        #region 关于Excel对象
        #region 关于图表
        #region 获取图表创建器
        public abstract ICreateExcelChart CreateChart { get; }
        #endregion
        #region 枚举工作表中的图表
        public abstract IEnumerable<IExcelObj<IOfficeChart>> Charts { get; }
        #endregion
        #endregion
        #region 关于图像
        #region 枚举工作表中的图像
        public abstract IEnumerable<IExcelObj<IImage>> Images { get; }
        #endregion
        #region 向工作表中添加图像
        public abstract IExcelObj<IImage> CreateImage(IImage image);
        #endregion
        #endregion
        #region 返回画布
        public abstract ICanvas Canvas { get; }
        #endregion
        #endregion
        #region 重写ToString方法
        public override string ToString()
            => Name;
        #endregion
        #region 构造方法
        /// <summary>
        /// 将指定的工作簿封装进工作表中
        /// </summary>
        /// <param name="book">指定的工作簿</param>
        public ExcelSheet(IExcelBook book)
        {
            this.Book = book;
        }
        #endregion
    }
}
