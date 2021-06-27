using System.Collections.Generic;
using System.Maths;

namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 在实现<see cref="IExcelCells"/>时，
    /// 可以继承自本类型，以减少重复的工作
    /// </summary>
    public abstract class ExcelCells : ExcelRange, IExcelCells
    {
        #region 返回IExcelCells接口
        /// <summary>
        /// 返回这个对象的接口形式，
        /// 它可以用来访问一些显式实现的成员
        /// </summary>
        protected IExcelCells Interface
            => this;
        #endregion
        #region 关于单元格本身
        #region 关于单元格内容
        #region 设置或者获取值
        public abstract RangeValue? Value { get; set; }
        #endregion
        #region 设置或者获取公式
        public abstract string? FormulaR1C1 { get; set; }
        #endregion
        #region 获取或设置超链接
        public abstract string? Link { get; set; }
        #endregion
        #endregion
        #region 获取完整地址
        public abstract (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; }
        #endregion
        #region 以文本形式返回地址（重写辅助方法）
        private protected override string AddressTextSimple(bool isR1C1)
            => ExcelRealize.GetAddress(Interface.AddressMath, !isR1C1);
        #endregion
        #region 返回视觉位置
        public abstract ISizePos VisualPosition { get; }
        #endregion
        #endregion 
        #region 关于子单元格和其他单元格
        #region 关于合并单元格
        #region 返回合并的单元格
        /// <summary>
        /// 如果<see cref="IsMerge"/>为<see langword="true"/>，则返回包含这个单元格的合并单元格，
        /// 如果为<see langword="false"/>，则返回这个单元格自己
        /// </summary>
        protected abstract IExcelCells MergeRange { get; }
        #endregion
        #region 合并和取消合并
        public abstract bool IsMerge { get; set; }
        #endregion
        #endregion
        #region 根据绝对位置返回单元格
        public abstract IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1] { get; }
        #endregion
        #region 枚举所有子单元格
        public abstract IEnumerable<IExcelCells> CellsAll { get; }
        #endregion
        #endregion
        #region 构造方法
        /// <summary>
        /// 用指定的工作表和地址初始化单元格
        /// </summary>
        /// <param name="sheet">指定的工作表</param>
        public ExcelCells(IExcelSheet sheet)
            : base(sheet)
        {

        }
        #endregion
    }
}
