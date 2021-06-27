using System.DrawingFrancis;
using System.DrawingFrancis.Text;

namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视作一个Excel单元格的样式
    /// </summary>
    public interface IRangeStyle
    {
        #region 背景颜色
        /// <summary>
        /// 设置或获取单元格的背景颜色，
        /// <see langword="null"/>代表无任何颜色
        /// </summary>
        IColor? BackColor { get; set; }

        /*本API的实现应当遵循以下规范：
          当读取这个属性时，返回单元格的背景颜色，如果无任何颜色，返回Null
          当写入这个属性时，修改单元格的背景颜色，如果写入Null，清除所有颜色
          写入这个属性时，不应该修改单元格的填充样式（如纯色，条纹等）*/
        #endregion
        #region 关于格式
        #region 数字格式
        /// <summary>
        /// 获取或设置数字格式
        /// </summary>
        string Format { get; set; }
        #endregion
        #region 文本格式
        /// <summary>
        /// 获取或设置单元格的文本格式
        /// </summary>
        ITextStyleVar TextStyle { get; set; }
        #endregion
        #endregion
        #region 关于对齐
        #region 获取或设置自动换行
        /// <summary>
        /// 获取或设置该单元格是否自动换行
        /// </summary>
        bool AutoLineBreaks { get; set; }
        #endregion
        #region 垂直对齐
        /// <summary>
        /// 获取或设置单元格的垂直对齐方式
        /// </summary>
        OfficeAlignment AlignmentVertical { get; set; }
        #endregion
        #region 水平对齐
        /// <summary>
        /// 获取或设置单元格的水平对齐方式
        /// </summary>
        OfficeAlignment AlignmentHorizontal { get; set; }
        #endregion
        #endregion 
    }
}
