using System.Maths;

namespace System.DrawingFrancis.Text
{
    /// <summary>
    /// 表示一个可设置的文本格式
    /// </summary>
    public interface ITextStyleVar : ITextStyle
    {
        #region 字体名称
        /// <summary>
        /// 获取或设置字体的名称
        /// </summary>
        new string FontName { get; set; }
        #endregion
        #region 字体的大小
        /// <summary>
        /// 获取或设置字体的大小
        /// </summary>
        new IUnit<IUTFontSize> Size { get; set; }
        #endregion
        #region 文本颜色
        /// <summary>
        /// 获取或设置文本的颜色
        /// </summary>
        new IColor TextColor { get; set; }
        #endregion
    }
}
