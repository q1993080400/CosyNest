namespace System.Office.Excel.Realize
{
    /// <summary>
    /// 在实现<see cref="IExcelRange"/>时，
    /// 可以继承自本类型，以减少重复的工作
    /// </summary>
    public abstract class ExcelRange : IExcelRange
    {
        #region 获取单元格所在的工作表
        public IExcelSheet Sheet { get; }
        #endregion
        #region 设置或获取样式
        public abstract IRangeStyle Style { get; set; }
        #endregion
        #region 返回单元格地址
        #region 辅助方法
        /// <summary>
        /// 获取范围地址的简单形式，它不包括文件名等信息
        /// </summary>
        /// <param name="isR1C1">如果这个值为<see langword="true"/>，
        /// 代表以R1C1形式返回，否则代表以A1形式返回</param>
        /// <returns></returns>
        private protected virtual string AddressTextSimple(bool isR1C1)
             => throw new NotImplementedException();
        #endregion
        #region 正式方法
        public virtual string AddressText(bool isR1C1 = true, bool isComplete = false)
        {
            var address = AddressTextSimple(isR1C1);
#pragma warning disable CA2208
            return isComplete ? ExcelRealize.GetAddressFull
                (Sheet.Book.Path ?? throw new ArgumentNullException("该工作簿尚未保存到文件中，无法获取单元格的完全路径"),
                Sheet.Name, address) : address;
#pragma warning restore
        }
        #endregion 
        #endregion
        #region 重写的ToString
        public sealed override string ToString()
            => AddressText();
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的工作表初始化对象
        /// </summary>
        /// <param name="sheet">这个范围所在的工作表</param>
        public ExcelRange(IExcelSheet sheet)
        {
            this.Sheet = sheet;
        }
        #endregion
    }
}
