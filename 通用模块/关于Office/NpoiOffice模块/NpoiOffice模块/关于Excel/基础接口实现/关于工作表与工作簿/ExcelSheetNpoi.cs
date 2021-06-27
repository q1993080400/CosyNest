using System.Collections.Generic;
using System.DrawingFrancis;
using System.DrawingFrancis.Graphics;
using System.Office.Chart;
using System.Office.Excel.Realize;

using NPOI.SS.UserModel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型代表使用Npoi实现的Excel工作表
    /// </summary>
    class ExcelSheetNpoi : ExcelSheet
    {
        #region 封装的对象
        #region 工作簿
        /// <summary>
        /// 获取工作表所在的工作簿
        /// </summary>
        private IWorkbook BookNpoi => Sheet.Workbook;
        #endregion
        #region 工作表
        /// <summary>
        /// 获取封装的Excel工作表，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        public ISheet Sheet { get; }
        #endregion
        #endregion 
        #region 关于工作表
        #region 返回工作表索引
        /// <summary>
        /// 返回工作表在工作簿中的索引
        /// </summary>
        private int Index
            => BookNpoi.GetSheetIndex(Sheet);
        #endregion
        #region 工作表名称
        public override string Name
        {
            get => Sheet.SheetName;
            set => BookNpoi.SetSheetName(Index, value);
        }
        #endregion
        #region 用户区域
        public override IExcelCells RangUser => throw new NotImplementedException();
        #endregion
        #region 获取行或列
        public override IExcelRC GetRC(int begin, int? end, bool isRow)
            => throw CreateException.NotSupported();
        #endregion
        #region 删除工作表
        public override void Delete()
            => BookNpoi.RemoveSheetAt(Index);
        #endregion
        #region 复制工作表
        public override IExcelSheet Copy(IExcelSheetCollection collection)
        {
            if (collection is ExcelSheetCollectionNpoi sheets)
            {
                Sheet.CopyTo(sheets.ExcelBookNpoi, ExcelRealize.SheetRepeat(collection, Name), true, false);
                return collection[^1];
            }
            throw new Exception($"{collection}不是Npoi实现的工作表集合");
        }
        #endregion
        #endregion
        #region 关于页面和Excel对象
        #region 返回页面对象
        public override IPageSheet Page => throw CreateException.NotSupported();
        #endregion
        #region 返回图表创建器
        public override ICreateExcelChart CreateChart => throw CreateException.NotSupported();
        #endregion
        #region 返回图表集合
        public override IEnumerable<IExcelObj<IOfficeChart>> Charts => throw CreateException.NotSupported();
        #endregion
        #region 返回图片集合
        public override IEnumerable<IExcelObj<IImage>> Images => throw CreateException.NotSupported();
        #endregion
        #region 创建图片
        public override IExcelObj<IImage> CreateImage(IImage image)
            => throw CreateException.NotSupported();
        #endregion
        #region 返回画布
        public override ICanvas Canvas => throw CreateException.NotSupported();
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="book">工作表所在的工作簿</param>
        /// <param name="sheet">封装的Npoi工作表</param>
        public ExcelSheetNpoi(IExcelBook book, ISheet sheet)
            : base(book)
        {
            Sheet = sheet;
        }
        #endregion
    }
}
