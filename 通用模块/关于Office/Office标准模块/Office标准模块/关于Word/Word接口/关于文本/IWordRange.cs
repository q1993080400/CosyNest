using System.DrawingFrancis.Text;

namespace System.Office.Word
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Word范围，
    /// 它代表只包含带格式文本的区域
    /// </summary>
    public interface IWordRange
    {
        #region 范围所属的文档
        /// <summary>
        /// 获取这个范围所属的文档
        /// </summary>
        IWordDocument Document { get; }
        #endregion
        #region 范围的开始和结束
        /// <summary>
        /// 这个属性指示范围的开始和结束
        /// </summary>
        (IWordBookmark Begin, IWordBookmark End) Range { get; }
        #endregion
        #region 获取或设置超链接
        /// <summary>
        /// 获取或设置这段文本的超级链接，
        /// 如果为<see langword="null"/>，
        /// 代表没有链接或存在多个链接
        /// </summary>
        string? Link { get; set; }
        #endregion
        #region 关于文本和格式
        #region 范围的文本
        /// <summary>
        /// 获取或设置范围的无格式文本
        /// </summary>
        string Text { get; set; }
        #endregion
        #region 范围的长度
        /// <summary>
        /// 获取范围的长度，也就是范围文本的字数
        /// </summary>
        int Length => Text.Length;
        #endregion
        #region 返回是否具有多种格式
        /// <summary>
        /// 返回这个范围内的文本是否具有多种样式
        /// </summary>
        bool HasMultipleStyle
            => Style == OfficeTextStyleCom.Multiple;
        #endregion
        #region 范围的文本格式
        /// <summary>
        /// 获取或设置范围的文本格式
        /// </summary>
        /// <exception cref="ArgumentException">写入了<see cref="OfficeTextStyleCom.Multiple"/></exception>
        ITextStyleVar Style { get; set; }

        /*实现本API请遵循以下规范：
          #如果这个范围的文本包含多种格式，
          则这个属性返回OfficeTextStyleCom.Multiple*/
        #endregion
        #endregion
        #region 删除范围
        /// <summary>
        /// 删除这个范围
        /// </summary>
        void Delete()
            => Text = "";
        #endregion
    }
}
