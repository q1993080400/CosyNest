using System.Office.Excel;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来封装块属性的读写方式
    /// </summary>
    public interface IBlockProperty
    {
        #region 读取属性
        /// <summary>
        /// 用于读取块属性的委托，
        /// 参数是属性所在的块，返回值是属性的值
        /// </summary>
        Func<IExcelCells, object?> GetValue { get; }
        #endregion
        #region 写入属性
        /// <summary>
        /// 用于写入块属性的委托，
        /// 参数分别是属性所在的块，以及属性的新值
        /// </summary>
        Action<IExcelCells, object?> SetValue { get; }
        #endregion
        #region 写入标题
        /// <summary>
        /// 用于写入标题的委托，参数是标题所在的单元格，单元格的大小与块相同，
        /// 如果为<see langword="null"/>，代表该属性没有标题
        /// </summary>
        Action<IExcelCells>? SetTitle { get; }
        #endregion
        #region 解构对象
        /// <summary>
        /// 将对象解构为读取属性的委托，写入属性和的委托和写入标题的委托
        /// </summary>
        /// <param name="getValue">用来接收读取属性的委托的对象</param>
        /// <param name="setValue">用来接收写入属性的委托的对象</param>
        /// <param name="setTitle">用来接收写入标题的委托的对象</param>
        void Deconstruct(out Func<IExcelCells, object?> getValue, out Action<IExcelCells, object?> setValue, out Action<IExcelCells>? setTitle)
        {
            getValue = this.GetValue;
            setValue = this.SetValue;
            setTitle = this.SetTitle;
        }
        #endregion
    }
}
