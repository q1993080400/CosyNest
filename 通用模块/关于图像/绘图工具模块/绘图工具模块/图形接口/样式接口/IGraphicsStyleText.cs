using System.DrawingFrancis.Text;

namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个画布上文字的样式
    /// </summary>
    public interface IGraphicsStyleText : IGraphicsStyle, ITextStyleVar
    {

    }
}
