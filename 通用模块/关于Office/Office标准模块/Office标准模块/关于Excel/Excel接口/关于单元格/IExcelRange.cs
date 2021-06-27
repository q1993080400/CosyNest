namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视作一个Excel范围
    /// </summary>
    public interface IExcelRange
    {
        #region 设置或获取样式
        /// <summary>
        /// 设置或者获取范围的样式
        /// </summary>
        IRangeStyle Style { get; set; }

        /*实现本API请遵循以下规范：
          当写入这个属性时，等同于复制格式*/
        #endregion
        #region 关于工作表，工作簿和单元格地址
        #region 获取单元格所在的工作表
        /// <summary>
        /// 获取单元格所在的工作表
        /// </summary>
        IExcelSheet Sheet { get; }
        #endregion
        #region 获取单元格所在的工作薄
        /// <summary>
        /// 获取单元格所在的工作薄
        /// </summary>
        IExcelBook Book
            => Sheet.Book;
        #endregion
        #region 返回单元格地址
        /// <summary>
        /// 以文本形式，返回单元格地址
        /// </summary>
        /// <param name="isR1C1">如果这个值为<see langword="true"/>，
        /// 代表以R1C1形式返回，否则代表以A1形式返回</param>
        /// <param name="isComplete">如果这个值为<see langword="true"/>，
        /// 代表返回完整地址，它包含了关于所在文件等信息，否则返回标准形式的地址</param>
        /// <returns></returns>
        string AddressText(bool isR1C1 = true, bool isComplete = false);
        #endregion
        #endregion
    }
}
