using System.Collections.Generic;
using System.Linq;
using System.Office.Excel.Realize;

using NPOI.SS.UserModel;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型是使用Npoi实现的Excel工作表集合
    /// </summary>
    class ExcelSheetCollectionNpoi : ExcelSheetCollection
    {
        #region 返回工作簿
        /// <summary>
        /// 返回本工作表集合所在的工作簿，
        /// 它以Npoi格式呈现
        /// </summary>
        public IWorkbook ExcelBookNpoi
            => ((ExcelBookNpoi)ExcelBook).WorkBook;
        #endregion
        #region 枚举所有工作表
        #region Npoi格式
        /// <summary>
        /// 以<see cref="ISheet"/>的形式枚举所有工作表
        /// </summary>
        private IEnumerable<ISheet> Sheets
        {
            get
            {
                using var enumerator = ExcelBookNpoi.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
        #endregion
        #region 自定义格式
        public override IEnumerator<IExcelSheet> GetEnumerator()
            => Sheets.Select(x => new ExcelSheetNpoi(ExcelBook, x)).GetEnumerator();
        #endregion
        #endregion
        #region 返回工作表的数量
        public override int Count
            => Sheets.Count();
        #endregion
        #region 关于添加和删除工作表
        #region 插入工作表
        public override void Insert(int index, IExcelSheet item)
        {
            if (item is ExcelSheetNpoi)
            {
                var newSheet = (ExcelSheetNpoi)item.Copy(this);
                ExcelBookNpoi.SetSheetOrder(newSheet.Name, index);
            }
            else throw new Exception($"{item}不是通过Npoi实现的工作表");
        }
        #endregion
        #region 添加工作表
        public override IExcelSheet Add(string name = "Sheet")
        {
            var sheet = ExcelBookNpoi.CreateSheet(ExcelRealize.SheetRepeat(this, name));
            return new ExcelSheetNpoi(ExcelBook, sheet);
        }
        #endregion
        #region 移除所有工作表
        public override void Clear()
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                ExcelBookNpoi.RemoveSheetAt(i);
            }
        }
        #endregion
        #endregion
        #region 构造函数
        /// <inheritdoc cref="ExcelSheetCollection(IExcelBook)"/>
        public ExcelSheetCollectionNpoi(IExcelBook ExcelBook)
            : base(ExcelBook)
        {

        }
        #endregion
    }
}
