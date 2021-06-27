using System.Maths;

namespace System.DrawingFrancis.Text
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个不可变的文本样式
    /// </summary>
    public interface ITextStyle
    {
        #region 字体名称
        /// <summary>
        /// 获取字体的名称
        /// </summary>
        string FontName { get; }
        #endregion
        #region 字体的大小
        /// <summary>
        /// 获取字体的大小
        /// </summary>
        IUnit<IUTFontSize> Size { get; }
        #endregion
        #region 文本颜色
        /// <summary>
        /// 这个属性指示文本的颜色
        /// </summary>
        IColor TextColor { get; }
        #endregion
        #region 返回一个可变的副本
        /// <summary>
        /// 返回一个本对象的可变副本
        /// </summary>
        /// <returns></returns>
        ITextStyleVar ToVar()
            => this is ITextStyleVar style ? style : new TextStyleVar(FontName, Size, TextColor);

        /*问：为什么需要本方法？
          答：因为作者意识到，很多ITextStyleVar属性的返回值是ITextStyleVar，
          但在写入的时候实际上写入ITextStyle就可以了，因为不可变的文本样式已经包含了样式的全部信息，
          因此作者提供了这个API，为这种情况提供方便*/
        #endregion
    }
}
