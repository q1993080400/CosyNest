using System.DrawingFrancis;
using System.DrawingFrancis.Text;
using System.Maths;

namespace System.Office
{
    /// <summary>
    /// 这个对象表示文本的各个部分具有不同的格式
    /// </summary>
    class TextStyleMultiple : ITextStyleVar
    {
        #region 返回异常
        /// <summary>
        /// 读写本类型的任何属性，
        /// 或调用任何方法时都会引发该异常
        /// </summary>
        private static NotImplementedException Exception
            => new("由于选定文本或范围的各个部分具有不同的文本格式，" +
                $"不支持读写该{nameof(ITextStyleVar)}的任何属性，或调用任何方法");
        #endregion
        #region 接口实现
        #region 字体名称
        public string FontName
        {
            get => throw Exception;
            set => throw Exception;
        }
        #endregion
        #region 字体的大小
        public IUnit<IUTFontSize> Size
        {
            get => throw Exception;
            set => throw Exception;
        }
        #endregion
        #region 文本颜色
        public IColor TextColor
        {
            get => throw Exception;
            set => throw Exception;
        }
        #endregion
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => "该文本的不同部分具有多个样式";
        #endregion
    }
}
